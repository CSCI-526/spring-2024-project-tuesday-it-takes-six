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
            GlobalData.HasReachedCheckpoint = true;
            GlobalData.LastCheckpointPosition = transform.position;

            Debug.Log($"Just past a check point with position {GlobalData.LastCheckpointPosition}");
        }
    }
}
