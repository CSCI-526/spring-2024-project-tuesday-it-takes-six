using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLauncherControll : MonoBehaviour
{

    private Vector3 lauchDirection = new Vector3(1,0,0);
    private float rayLength = 10.0f;
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
        	lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
            lineDrawer.DrawLine(transform.position, transform.position+rayLength*lauchDirection);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
