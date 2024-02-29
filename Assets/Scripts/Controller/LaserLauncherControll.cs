using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class LaserLauncherControll : MonoBehaviour
{

    [SerializeField] private Vector3 lauchDirection = new Vector3(1,0,0);
    [SerializeField] private float rayLength = 15.0f;
    [SerializeField] private GameObject[] timePortals;
    private float rotateAngle = 5.0f;

    private LineDrawer lineDrawer;
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

    private bool HitEnemy(RaycastHit2D[] hitObjs, out GameObject bodyObj)
    {
        bodyObj = null;
        foreach (RaycastHit2D hitObj in hitObjs)
        {
            if (hitObj.collider.tag == "Portal")
            {
                return false;
            }
        }
        return false;
    }

    private void HitDetect()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lauchDirection, rayLength, 1<<0);
        GameObject bodyObj;
        if (HitEnemy(hits, out bodyObj))
        {
            // Debug.Log("Hit one enemy ");
            bodyObj.SendMessage("Die");
        }
        else
        {
            // detect the collision with time portals
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
                    Debug.Log("laser collide portal");
                    // redraw the line
                    lineDrawer.DrawLine(transform.position, intersectPos);
                    // send line to the other time dimension
                    object[] para = new object[3];
                    para[0] = lauchDirection;
                    para[1] = rayLength;
                    para[2] = intersectPos;
                    portalObj.SendMessage("TransferLaser", para, SendMessageOptions.RequireReceiver);
                }
            }
        }
    }

    private void DrawLaser()
    {
        lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
        lineDrawer.DrawLine(transform.position, transform.position+rayLength*lauchDirection);
    }

    private void DetectRotate()
    {
        if (Input.GetButtonDown("LaserRotate"))
        {
            lauchDirection = Utils.RotateRound(lauchDirection, new Vector3(0, 0, 0), Vector3.forward, rotateAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectRotate();
        DrawLaser();
        HitDetect();
    }
}
