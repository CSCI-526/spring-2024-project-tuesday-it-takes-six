using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;

public class TimePortalController : MonoBehaviour, IChangeable
{

    private LineDrawer lineDrawer;
    private TimeTense currentTimeTense;
    private TimeTense laserTimeTense;

    private Vector3 lauchDirection;
    private float rayLength = 0.0f;
    private Vector3 lauchStartPoint;

    private float currentTime = 0.0f;
    private float sleepDuration = 5.0f;
	private float activeDuration = 5.0f;
    private bool isActive = false;
    private GameObject portalUI;

    private struct HitInfo
    {
        public GameObject hitObj;
        public Vector3 hitPoint;
        public float hitDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        portalUI = this.transform.GetChild(0).gameObject;
        currentTimeTense = TimeTense.PRESENT;
        portalUI.GetComponent<SpriteRenderer>().color = Color.black;
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive)
        {
            if (other.CompareTag("Corpse"))
            {
                GameObject enemyObj = other.transform.parent.gameObject;
                float xDistance = (enemyObj.transform.position.x - transform.position.x)*2.5f;
                if (enemyObj.transform.parent.name != "Common")
                {
                    Debug.Log(enemyObj.transform.parent.name);
                    enemyObj.transform.position = new Vector3(enemyObj.transform.position.x-xDistance, enemyObj.transform.position.y, 0.0f);
                    
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lauchDirection, rayLength, 1<<0);
        if (hits.Length==0)
        {
            return false;
        }
        // Debug.Log(hit.collider.transform.name);
        // Debug.Log(hit.point);
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
        hitInfo.hitDistance = Mathf.Sqrt((transform.position - hitInfo.hitPoint).sqrMagnitude);
        return true; 
    }

    private void HitDetect()
    {
        HitInfo hitPhysicalInfo = new HitInfo();
        bool hitWithPhysicalObj = HitPhysicalObject(out hitPhysicalInfo);

        if (hitWithPhysicalObj)
        {
            DrawLaser(hitPhysicalInfo.hitPoint);
            switch (hitPhysicalInfo.hitObj.tag)
            {
                case "Enemy":
                {
                    // Debug.Log("Laser hit Enemy");
                    Debug.Log(hitPhysicalInfo.hitObj.name);
                    hitPhysicalInfo.hitObj.transform.parent.gameObject.SendMessage("Die");
                    break;
                }
                case "Player":
                {
                    // kill player, it is the PlayerRB be hit
                    // hitPhysicalInfo.hitObj.transform.parent.gameObject.SendMessage("SetDeath", true);
                    GlobalData.PlayerStatusData.KillPlayer();
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
            DrawLaser(transform.position+rayLength*lauchDirection);
        }

    }

    public void TransferLaser(object[] drawInfo)
    {
        lauchDirection = (Vector3) drawInfo[0];
        rayLength = (float) drawInfo[1];
        lauchStartPoint = (Vector3) drawInfo[2];
        if (isActive)
        {
            Debug.Log("transfer laser");
            laserTimeTense = currentTimeTense == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
        }
        else
        {
            Debug.Log("keep laser");
            laserTimeTense = currentTimeTense;
            HitDetect();
        }
    }

    public void LaserGone()
    {
        Debug.Log("LaserGone");
        lineDrawer.ClearLine();
        rayLength = -1.0f;
    }


    public void OnPresent()
    {
        if (isActive && laserTimeTense == TimeTense.PRESENT && rayLength > 0)
        {
            HitDetect();
        }
        else 
        {
            lineDrawer.ClearLine();
        }
    }

    public void OnPast()
    {
        if (isActive && laserTimeTense == TimeTense.PAST && rayLength > 0)
        {
            HitDetect();
        }
        else 
        {
            lineDrawer.ClearLine();
        }
    }

    private void PeriodActive()
    {
        if (currentTime < sleepDuration)
        {
            currentTime += Time.deltaTime;  
        }
        else {
            portalUI.GetComponent<SpriteRenderer>().color = Color.green;
            isActive = true;
            lineDrawer.ClearLine();
            if (currentTime < sleepDuration + activeDuration)
            {
                currentTime += Time.deltaTime;  
            }
            else 
            {
                isActive = false;
                lineDrawer.ClearLine();
                portalUI.GetComponent<SpriteRenderer>().color = Color.black;
                currentTime = 0.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PeriodActive();
    }
}
