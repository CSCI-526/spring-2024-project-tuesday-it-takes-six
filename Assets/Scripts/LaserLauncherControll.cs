using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLauncherControll : MonoBehaviour
{

    private Vector3 lauchDirection = new Vector3(1,0,0);
    private float rayLength = 5.0f;
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

    private void HitDetect()
    {
    	RaycastHit2D hit = Physics2D.Raycast(transform.position, lauchDirection, rayLength, 1<<0);
    	if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.transform.name);
       		if (hit.transform.name == "Enemy")
       		{
       			GameObject enemyObj = GameObject.Find("Enemy");
            enemyObj.SendMessage("Die");
       		}
       		if (hit.transform.name == "TimePortal")
       		{
       			GameObject portalObj = GameObject.Find("TimePortal");
       			// TODO: call TransferLaser() in TimePortalController
            portalObj.SendMessage("TransferLaser", new object[] {lauchDirection, rayLength});
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
