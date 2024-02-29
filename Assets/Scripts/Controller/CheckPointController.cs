using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
public class CheckPointController : MonoBehaviour
{
    // Start is called before the first frame update
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
            // get current check point id
            // int id = GetInstanceID();

            GlobalData.HasReachedCheckpoint = true;
            GlobalData.LastCheckpointPosition = transform.position;

            // TODO: save progress by check point id. If player died later, revive the player to the last checkpoint
            // Debug.Log($"Just past a check point with id #{id}");
            Debug.Log($"Just past a check point with position {GlobalData.LastCheckpointPosition}");
        }
    }
}
