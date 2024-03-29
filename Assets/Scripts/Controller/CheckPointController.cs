using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using Game;

public class CheckPointController : MonoBehaviour
{

    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 position = transform.position;
            GlobalData.CheckPointData.SetLastCheckPointPosition(position);

            Debug.Log($"Just past a check point with position {position}");


            GlobalData.AnalyticsManager.Send("passedCheckpoint");
    
            Debug.Log("checkpoint event submitted");
        }
    }

}
