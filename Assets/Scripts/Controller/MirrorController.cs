using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;


public class MirrorController : ResetableMonoBehaviour
{   
    private LineDrawer lineDrawer;

    private Vector3 lauchDirection;
    private float rayLength = 0.0f;
    private Vector3 lauchStartPoint;
    private TimeTense laserTimeTense;
    // private bool hitPlayer = false;

    private struct HitInfo
    {
        public GameObject hitObj;
        public Vector3 hitPoint;
        public float hitDistance;
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(lauchStartPoint, lauchDirection, rayLength, 1<<0);
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
                    // if (!hitPlayer)
                    // {
                        GlobalData.PlayerStatusData.KillPlayer();
                        // hitPlayer = true;
                    // }
                    break;
                }
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

    public void ReflectLaser(object[] drawInfo)
    {
    	lauchDirection = (Vector3) drawInfo[0];
        rayLength = (float) drawInfo[1];
        lauchStartPoint = (Vector3) drawInfo[2];
        laserTimeTense = GlobalData.TimeTenseData.GetTimeTense();
    }

    public void LaserGone()
    {
        lineDrawer.ClearLine();
        rayLength = -1.0f;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        lineDrawer = GetComponent<LineDrawer>();
    }

    public override void OnReset(bool r)
    {
        // hitPlayer = false;
        LaserGone();
    }

    private void LaserCheck()
    {
        if (laserTimeTense == GlobalData.TimeTenseData.GetTimeTense() && rayLength > 0)
        {
            HitDetect();
        }
        else
        {
            lineDrawer.ClearLine();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LaserCheck();
    }
}
