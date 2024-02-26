using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;

public class TimePortalController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    
    private LineDrawer lineDrawer;
    private TimeTense currentTime;
    private TimeTense laserTime;

    private Vector3 lauchDirection;
    private float rayLength = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = TimeTense.PRESENT;
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
    }

    private void DrawLaser()
    {
      lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
      lineDrawer.DrawLine(transform.position, transform.position+rayLength*lauchDirection);
    }

    public void TransferLaser(Vector3 direction, float transferLength)
    {
        laserTime = currentTime == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
        lauchDirection = direction;
        rayLength = transferLength;
    }


    public void OnPresent()
    {
        if (laserTime == TimeTense.PRESENT && rayLength > 0)
        {
            DrawLaser();
        }
    }

    public void OnPast()
    {
        if (laserTime == TimeTense.PAST && rayLength > 0)
        {
            DrawLaser();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
