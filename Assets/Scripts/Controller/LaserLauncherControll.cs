using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class LaserLauncherControll : MonoBehaviour
{

    [SerializeField] private Vector3 lauchDirection = new Vector3(1,0,0);
    [SerializeField] private float rayLength = 15.0f;
    [SerializeField] private GameObject[] timePortals;
    private float rotateAngle = 45.0f;

    private LineDrawer lineDrawer;

    private struct HitInfo
    {
        public GameObject hitObj;
        public Vector3 hitPoint;
        public float hitDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
        if (lineDrawer == null)
        {
            Debug.LogError("LineDrawer component not found on this GameObject.");
        }
    }

    private bool HitTimePortal(out HitInfo hitInfo)
    {
        hitInfo.hitDistance = 1000.0f;
        hitInfo.hitObj = null;
        hitInfo.hitPoint = new Vector3();
        bool hitFlag = false;
        foreach (GameObject portalObj in timePortals)
        {
            Vector3 portalHeight = new Vector3(0, 1.0f, 0); // better to read from portalObj

            Vector3 laserStart = transform.position;
            Vector3 laserEnd = transform.position+rayLength*lauchDirection;
            Vector3 portalStart = portalObj.transform.position + portalHeight;
            Vector3 portalEnd = portalObj.transform.position - portalHeight;

            Vector3 intersectPos;
            bool isIntersect = Utils.IsSegmentsIntersect(laserStart, laserEnd, portalStart, portalEnd, out intersectPos);
            if (isIntersect)
            {
                hitFlag = true;
                // Debug.Log("laser collide portal");
                float currentDistance = Mathf.Sqrt((transform.position - intersectPos).sqrMagnitude);
                if (currentDistance < hitInfo.hitDistance)
                {
                    hitInfo.hitObj = portalObj;
                    hitInfo.hitDistance = currentDistance;
                    hitInfo.hitPoint = intersectPos;
                }
            }
        }
        return hitFlag;
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
        HitInfo hitPortalInfo = new HitInfo();
        HitInfo hitPhysicalInfo = new HitInfo();
        bool hitWithPortal = HitTimePortal(out hitPortalInfo);
        bool hitWithPhysicalObj = HitPhysicalObject(out hitPhysicalInfo);
        Dictionary<string, HitInfo> hitCollection = new Dictionary<string, HitInfo>()
        {
            { "Portal", hitPortalInfo },
            { "Physical", hitPhysicalInfo }
        };

        if (hitWithPortal || hitWithPhysicalObj)
        {
            string hitKey = (hitCollection["Portal"].hitDistance < hitCollection["Physical"].hitDistance) ? "Portal" : "Physical";
            
            DrawLaser(hitCollection[hitKey].hitPoint);
            switch (hitCollection[hitKey].hitObj.tag)
            {
                case "Enemy":
                {
                    // Debug.Log("Laser hit Enemy");
                    Debug.Log(hitCollection[hitKey].hitObj.name);
                    // TODO: why the collider box of Enemy is smaller than body
                    hitCollection[hitKey].hitObj.transform.parent.gameObject.SendMessage("Die");
                    break;
                }
                case "Portal":
                {
                    // Debug.Log("Laser hit Time Portal");
                    object[] para = new object[3];
                    para[0] = lauchDirection;
                    para[1] = rayLength - hitCollection[hitKey].hitDistance;
                    para[2] = hitCollection[hitKey].hitPoint;
                    hitCollection[hitKey].hitObj.SendMessage("TransferLaser", para, SendMessageOptions.RequireReceiver);
                    break;
                }
                case "Player":
                {
                    // Debug.Log("Laser hit Player");
                    // kill player, it is the PlayerRB be hit
                    hitCollection[hitKey].hitObj.transform.parent.gameObject.SendMessage("SetDeath", true);
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
        if (PlayerIsClose() && Input.GetButtonDown("LaserRotate"))
        {
            lauchDirection = Utils.RotateRound(lauchDirection, new Vector3(0, 0, 0), Vector3.forward, rotateAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectRotate();
        HitDetect();
    }
}
