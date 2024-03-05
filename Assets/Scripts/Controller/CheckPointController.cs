using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.Analytics;


public class CheckPointController : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
 
        

            GlobalData.HasReachedCheckpoint = true;
            GlobalData.LastCheckpointPosition = transform.position;

            Debug.Log($"Just past a check point with position {GlobalData.LastCheckpointPosition}");

            var eventData = new Dictionary<string, object>();
            eventData["CheckpointPosition"] = GlobalData.LastCheckpointPosition;

            Analytics.CustomEvent("CheckpointPassed", eventData);
            Analytics.FlushEvents();
        }
    }

}
