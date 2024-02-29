using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class LaserLauncherControll : MonoBehaviour
{

    [SerializeField] private Vector3 lauchDirection = new Vector3(1,0,0);
    [SerializeField] private float rayLength = 15.0f;
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
        else
        {
            DrawLaser();
        }
    }

    private void HitDetect()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lauchDirection, rayLength, 1<<0);
        if (hit.collider != null && hit.transform.name == "Enemy")
        {
            GameObject enemyObj = GameObject.Find("Enemy");
            enemyObj.SendMessage("Die");
        }
        else
        {
            // detect the collision with time portal
            GameObject portalObj = GameObject.Find("TimePortal");
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

    private void DrawLaser()
    {
        lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
        lineDrawer.DrawLine(transform.position, transform.position+rayLength*lauchDirection);
    }

    // Update is called once per frame
    void Update()
    {
        DrawLaser();
        HitDetect();
    }
}
