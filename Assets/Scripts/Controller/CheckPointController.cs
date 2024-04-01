using UnityEngine;
using Game;
using System;

public class CheckPointController : MonoBehaviour
{

    public void Start()
    {
        string parentName = gameObject.transform.parent?.name ?? "Root";
        if (parentName != "Present" && parentName != "Common")
        {
            throw new Exception($"Checkpoint should be put under `Present` or `Common` and cannot be placed at `{parentName}`");
        }
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
