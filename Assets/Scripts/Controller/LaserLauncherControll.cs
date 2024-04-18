using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class LaserLauncherControll : MonoBehaviour
{

    [SerializeField] private Vector3 lauchDirection = new Vector3(1,0,0);
    [SerializeField] private float rayLength = 15.0f;
    [SerializeField] private GameObject[] relatedTo;
    [SerializeField] private float rotateAngle = 45.0f;

    private Vector3 initialDirection;
    private LineDrawer lineDrawer;

    private GameObject activeUI;
    // private bool hitPlayer = false;

    private struct HitInfo
    {
        public GameObject hitObj;
        public Vector3 hitPoint;
        public float hitDistance;
        public Vector3 hitNormal;
    }

    // Start is called before the first frame update
    public void Start()
    {
        // base.Start();
        Physics2D.queriesStartInColliders = false;
        activeUI = this.transform.GetChild(0).gameObject;
        activeUI.SetActive(false);
        initialDirection = lauchDirection;
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component not found on this GameObject.");
        }
    }

    // public override void OnRespawn(bool r)
    // {
    //     lauchDirection = initialDirection;
        // hitPlayer = false;
    // }

    private bool HitPhysicalObject(out HitInfo hitInfo)
    {
        hitInfo.hitDistance = 1000.0f;
        hitInfo.hitObj = null;
        hitInfo.hitPoint = new Vector3();
        hitInfo.hitNormal = new Vector3();
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
        hitInfo.hitNormal = hit.normal;
        return true; 
    }

    private void ClearTransferredLaser()
    {
        if (relatedTo.Length > 0)
        {
            foreach (GameObject relatedObj in relatedTo)
            {
                relatedObj.SendMessage("LaserGone");
            }
        }
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
                case "Portal":
                {
                    // Debug.Log("Laser hit Time Portal");
                    object[] para = new object[3];
                    para[0] = lauchDirection;
                    para[1] = rayLength - hitPhysicalInfo.hitDistance;
                    para[2] = hitPhysicalInfo.hitPoint;
                    hitPhysicalInfo.hitObj.SendMessage("TransferLaser", para, SendMessageOptions.RequireReceiver);
                    break;
                }
                case "Enemy":
                {
                    // Debug.Log("Laser hit Enemy");
                    Debug.Log(hitPhysicalInfo.hitObj.name);
                    hitPhysicalInfo.hitObj.transform.parent.gameObject.SendMessage("Die");
                    ClearTransferredLaser();
                    break;
                }
                case "Player":
                {
                    // kill player, it is the PlayerRB be hit
                    // hitPhysicalInfo.hitObj.transform.parent.gameObject.SendMessage("SetDeath", true);
                    // Debug.Log($"Hit player? {hitPlayer}");
                    // if (!hitPlayer)
                    // {
                        GlobalData.PlayerStatusData.KillPlayer();
                        ClearTransferredLaser();
                        // hitPlayer = true;
                    // }
                    break;
                }
                case "Mirror":
                {
                    Vector3 reflectDirection = Vector3.Reflect(lauchDirection, hitPhysicalInfo.hitNormal);
                    object[] para = new object[3];
                    para[0] = reflectDirection;
                    para[1] = rayLength - hitPhysicalInfo.hitDistance;
                    para[2] = hitPhysicalInfo.hitPoint;
                    hitPhysicalInfo.hitObj.SendMessage("ReflectLaser", para, SendMessageOptions.RequireReceiver);
                    break;
                }
                default:
                {
                    ClearTransferredLaser();
                    break;
                }
            }
        }
        else
        {
            ClearTransferredLaser();
            DrawLaser(transform.position+rayLength*lauchDirection);
        }

    }

    private void DrawLaser(Vector3 destination)
    {
        lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
        lineDrawer.DrawLine(transform.position, destination);
    }

    private bool PlayerIsClose()
    {
        GameObject player = GameObject.Find("PlayerRB");
        Vector3 playerPos = player.transform.position;
        float distance = Vector3.Distance(playerPos, transform.position);
        return (distance <= 2.0f);
    }

    private void DetectRotate()
    {
        if (PlayerIsClose())
        {
            activeUI.SetActive(true);
            if (Input.GetButtonDown("LaserRotateAntiClock"))
            {
                lauchDirection = Utils.RotateRound(lauchDirection, new Vector3(0, 0, 0), Vector3.forward, rotateAngle);
                ClearTransferredLaser();
            }
            else if (Input.GetButtonDown("LaserRotateClock"))
            {
                lauchDirection = Utils.RotateRound(lauchDirection, new Vector3(0, 0, 0), -Vector3.forward, rotateAngle);
                ClearTransferredLaser();
            }
        }
        else
        {
            activeUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectRotate();
        HitDetect();
    }
}
