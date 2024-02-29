using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    public GameObject rightBoundary;

    public float rightmostPosition;

    void Start()
    {
        rightmostPosition = rightBoundary.transform.position.x - 8;
    }

    void LateUpdate()
    {
        // only follow x and z, make y a constant for better view
        transform.position = new Vector3(
            Mathf.Min(player.transform.position.x + 2, rightmostPosition),
            3,
            player.transform.position.z - 1
        );
    }
}
