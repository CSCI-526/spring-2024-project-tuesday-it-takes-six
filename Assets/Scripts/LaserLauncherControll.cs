using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLauncherControll : MonoBehaviour
{

    private Vector3 lauchDirection = new Vector3(1,0,0);
    private float rayLength = 15.0f;
    private LineDrawer lineDrawer;
    // Start is called before the first frame update
    void Start()
    {
        // Note: delete this when it is available to modify the scene!
        transform.localScale = new Vector3(1,1,1);
        transform.position = new Vector3(39, (float) 0.5, 0);

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


    private bool IsSegmentsIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersectPos)
    {
        intersectPos = Vector3.zero;

        Vector3 ab = b - a;
        Vector3 ca = a - c;
        Vector3 cd = d - c;

        Vector3 v1 = Vector3.Cross(ca, cd);

        if (Mathf.Abs(Vector3.Dot(v1, ab)) > 1e-6)
        {
            return false;
        }

        if (Vector3.Cross(ab, cd).sqrMagnitude <= 1e-6)
        {
            return false;
        }

        Vector3 ad = d - a;
        Vector3 cb = b - c;

        if (Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) || Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x)
                || Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y) || Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y)
                || Mathf.Min(a.z, b.z) > Mathf.Max(c.z, d.z) || Mathf.Max(a.z, b.z) < Mathf.Min(c.z, d.z)
           )
            return false;

        if (Vector3.Dot(Vector3.Cross(-ca, ab), Vector3.Cross(ab, ad)) > 0
                && Vector3.Dot(Vector3.Cross(ca, cd), Vector3.Cross(cd, cb)) > 0)
        {
            Vector3 v2 = Vector3.Cross(cd, ab);
            float ratio = Vector3.Dot(v1, v2) / v2.sqrMagnitude;
            intersectPos = a + ab * ratio;
            return true;
        }

        return false;
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
            bool isIntersect = IsSegmentsIntersect(laserStart, laserEnd, portalStart, portalEnd, out intersectPos);
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
