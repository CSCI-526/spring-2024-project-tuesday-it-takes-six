using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;

public class TimePortalController : ResetableMonoBehaviour
{

    private LineDrawer lineDrawer;
    private TimeTense currentTimeTense;
    private TimeTense laserTimeTense;

    private Vector3 lauchDirection;
    private float rayLength = 0.0f;
    private Vector3 lauchStartPoint;

    private float currentTime = 0.0f;
    [SerializeField] private float sleepDuration = 2.0f;
	[SerializeField] private float activeDuration = 2.0f;
    private bool isActive = false;
    private bool corpseInPortal = false;
    private int corpseEnterDirection;
    private GameObject enemyObj;
    private GameObject portalUI;
    private int laserType = 0; // 0: no laser; 1: transfer; 2:pass
    private bool hitPlayer = false;

    private struct HitInfo
    {
        public GameObject hitObj;
        public Vector3 hitPoint;
        public float hitDistance;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        portalUI = this.transform.GetChild(0).gameObject;
        currentTimeTense = TimeTense.PRESENT;
        portalUI.GetComponent<SpriteRenderer>().color = Color.black;
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
    }

    public override void OnReset(bool r)
    {
        hitPlayer = false;
        LaserGone();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Corpse"))
        {
            enemyObj = other.transform.parent.gameObject;
            corpseInPortal = true;
            corpseEnterDirection = (transform.position.x - enemyObj.transform.position.x) > 0 ? 1 : -1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Corpse"))
        {
            corpseInPortal = false;
        }
    }

    private void TransferCorpse()
    {
        if (corpseInPortal && isActive)
        {
            float xDistance = enemyObj.transform.localScale.x/2.0f + 0.2f;
            enemyObj.transform.position = new Vector3(transform.position.x + xDistance * corpseEnterDirection, enemyObj.transform.position.y, 0.0f);
            enemyObj.gameObject.SetActive(false);
            if (enemyObj.transform.parent.name == "Present")
            {
                enemyObj.transform.parent = GameObject.Find("Past").transform;
            }
            else 
            {
                enemyObj.transform.parent = GameObject.Find("Present").transform;
            }
        }
    }

    private void DrawLaser(Vector3 destination)
    {
        lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
        lineDrawer.DrawLine(lauchStartPoint, destination);
    }

    private bool HitPhysicalObject(out HitInfo hitInfo)
    {
        hitInfo.hitDistance = 1000.0f;
        hitInfo.hitObj = null;
        hitInfo.hitPoint = new Vector3();
        RaycastHit2D[] hits = Physics2D.RaycastAll(lauchStartPoint+0.1f*lauchDirection, lauchDirection, rayLength, 1<<0);
        if (hits.Length==0)
        {
            return false;
        }

        RaycastHit2D hit = hits[0];
        if (hit.collider.tag == "Checkpoint")
        {
            if (hits.Length > 1)
            {
                hit = hits[1];
            }
            else
            {
                return false;
            }
        }
        hitInfo.hitObj = hit.collider.gameObject;
        hitInfo.hitPoint = new Vector3(hit.point.x, hit.point.y, 0);
        hitInfo.hitDistance = Mathf.Sqrt((lauchStartPoint - hitInfo.hitPoint).sqrMagnitude);
        return true; 
    }

    private void HitDetect()
    {
        HitInfo hitPhysicalInfo = new HitInfo();
        bool hitWithPhysicalObj = HitPhysicalObject(out hitPhysicalInfo);

        if (hitWithPhysicalObj)
        {
            DrawLaser(hitPhysicalInfo.hitPoint);
            // Debug.Log(hitPhysicalInfo.hitObj.name);
            switch (hitPhysicalInfo.hitObj.tag)
            {
                case "Enemy":
                {
                    hitPhysicalInfo.hitObj.transform.parent.gameObject.SendMessage("Die");
                    break;
                }
                case "Player":
                {
                    if (!hitPlayer)
                    {
                        GlobalData.PlayerStatusData.KillPlayer();
                        hitPlayer = true;
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        else
        {
            DrawLaser(lauchStartPoint+rayLength*lauchDirection);
        }

    }

    public void TransferLaser(object[] drawInfo)
    {
        lauchDirection = (Vector3) drawInfo[0];
        rayLength = (float) drawInfo[1];
        lauchStartPoint = (Vector3) drawInfo[2];
        if (isActive)
        {
            laserTimeTense = currentTimeTense == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
            laserType = 1;
        }
        else
        {
            laserTimeTense = currentTimeTense;
            laserType = 2;
            HitDetect();
        }
    }

    public void LaserGone()
    {
        lineDrawer.ClearLine();
        rayLength = -1.0f;
    }


    private void PeriodActive()
    {
        if (currentTime < sleepDuration)
        {
            currentTime += Time.deltaTime;  
        }
        else {
            portalUI.GetComponent<SpriteRenderer>().color = new Color(0f , 1f, 0f, 1f);
            isActive = true;
            if (currentTime < sleepDuration + activeDuration)
            {
                currentTime += Time.deltaTime;  
            }
            else 
            {
                isActive = false;
                portalUI.GetComponent<SpriteRenderer>().color = new Color(0f , 1f, 0f, 0f);
                currentTime = 0.0f;
            }
        }
    }

    private void LaserCheck()
    {
        if (laserTimeTense == GlobalData.TimeTenseData.GetTimeTense() && rayLength > 0)
        {
            if (laserType != 1 || isActive)
            {
                HitDetect();
            }
            else
            {
                lineDrawer.ClearLine();
            }
        }
        else
        {
            lineDrawer.ClearLine();
        }
    }
    // Update is called once per frame
    void Update()
    {
        currentTimeTense = GlobalData.TimeTenseData.GetTimeTense();
        PeriodActive();
        LaserCheck();
        TransferCorpse();
    }
}
