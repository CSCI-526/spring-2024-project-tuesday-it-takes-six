using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;

public class TimePortalController : MonoBehaviour, IChangeable
{

    private LineDrawer lineDrawer;
    private TimeTense currentTimeTense;
    private TimeTense laserTimeTense;

    private Vector3 lauchDirection;
    private float rayLength = 0.0f;
    private Vector3 lauchStartPoint;

    private float currentTime = 0.0f;
    private float sleepDuration = 5.0f;
	private float activeDuration = 5.0f;
    private bool isActive = false;
    private GameObject portalUI;

    // Start is called before the first frame update
    void Start()
    {
        portalUI = this.transform.GetChild(0).gameObject;
        currentTimeTense = TimeTense.PRESENT;
        portalUI.GetComponent<SpriteRenderer>().color = Color.black;
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive)
        {
            if (other.CompareTag("Corpse"))
            {
                GameObject enemyObj = other.transform.parent.gameObject;
                float xDistance = (enemyObj.transform.position.x - transform.position.x)*2.5f;
                if (enemyObj.transform.parent.name != "Common")
                {
                    Debug.Log(enemyObj.transform.parent.name);
                    enemyObj.transform.position = new Vector3(enemyObj.transform.position.x-xDistance, enemyObj.transform.position.y, 0.0f);
                    
                    enemyObj.gameObject.SetActive(false);
                    if (enemyObj.transform.parent.name == "Present")
                    {
                        
                        enemyObj.transform.parent = GameObject.Find("Past").transform;
                    }
                    else 
                    {
                        enemyObj.transform.parent = GameObject.Find("Present").transform;
                    }
                }
            }
        }
    }

    private void DrawLaser()
    {
        lineDrawer.SetLineStyle(0.05f, 0.05f, Color.yellow);
        lineDrawer.DrawLine(lauchStartPoint, lauchStartPoint+rayLength*lauchDirection);
    }

    private void HitDetect()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lauchDirection, rayLength, 1<<0);
        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            GameObject enemyObj = hit.transform.parent.gameObject;
            enemyObj.SendMessage("Die");
        }
    }

    public void TransferLaser(object[] drawInfo)
    {
        Debug.Log("transfer laser");
        laserTimeTense = currentTimeTense == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
        lauchDirection = (Vector3) drawInfo[0];
        rayLength = (float) drawInfo[1];
        lauchStartPoint = (Vector3) drawInfo[2];
    }

    public void LaserGone()
    {
        Debug.Log("LaserGone");
        lineDrawer.ClearLine();
        rayLength = -1.0f;
    }


    public void OnPresent()
    {
        if (isActive && laserTimeTense == TimeTense.PRESENT && rayLength > 0)
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
        if (isActive && laserTimeTense == TimeTense.PAST && rayLength > 0)
        {
            DrawLaser();
            HitDetect();
        }
        else 
        {
            lineDrawer.ClearLine();
        }
    }

    private void PeriodActive()
    {
        if (currentTime < sleepDuration)
        {
            currentTime += Time.deltaTime;  
        }
        else {
            portalUI.GetComponent<SpriteRenderer>().color = Color.green;
            isActive = true;
            if (currentTime < sleepDuration + activeDuration)
            {
                currentTime += Time.deltaTime;  
            }
            else 
            {
                isActive = false;
                portalUI.GetComponent<SpriteRenderer>().color = Color.black;
                currentTime = 0.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PeriodActive();
    }
}
