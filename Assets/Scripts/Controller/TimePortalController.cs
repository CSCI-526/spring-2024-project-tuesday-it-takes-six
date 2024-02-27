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
    private Vector3 lauchStartPoint;
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
        lineDrawer.DrawLine(lauchStartPoint, lauchStartPoint+rayLength*lauchDirection);
    }

    private void HitDetect()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lauchDirection, rayLength, 1<<0);
        if (hit.collider != null && hit.transform.name == "Enemy")
        {
            GameObject enemyObj = GameObject.Find("Enemy");
            enemyObj.SendMessage("Die");
        }
    }

    public void TransferLaser(object[] drawInfo)
    {
        Debug.Log("transfer laser");
        laserTime = currentTime == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
        lauchDirection = (Vector3) drawInfo[0];
        rayLength = (float) drawInfo[1];
        lauchStartPoint = (Vector3) drawInfo[2];
    }


    public void OnPresent()
    {
        Debug.Log("OnPresent");
        if (laserTime == TimeTense.PRESENT && rayLength > 0)
        {
            DrawLaser();
            HitDetect();
        }
        else 
        {
            lineDrawer.ClearLine();
        }
    }

    public void OnPast()
    {
        Debug.Log("OnPast");
        if (laserTime == TimeTense.PAST && rayLength > 0)
        {
            DrawLaser();
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

    }
}
