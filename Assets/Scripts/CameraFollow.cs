using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        // only follow x and z, make y a constant for better view
        transform.position = new Vector3(player.transform.position.x + 2, 3, player.transform.position.z - 1);
    }
}
