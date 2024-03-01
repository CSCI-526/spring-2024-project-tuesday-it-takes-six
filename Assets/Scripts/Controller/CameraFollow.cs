using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject playerRB;

    [SerializeField]
    private GameObject rightBoundary;

    private float rightmostPosition;

    void Start()
    {
        rightmostPosition = rightBoundary.transform.position.x - 8;
    }

    void LateUpdate()
    {
        // only follow x and z, make y a constant for better view
        transform.position = new Vector3(
            Mathf.Min(playerRB.transform.position.x + 2, rightmostPosition),
            3,
            playerRB.transform.position.z - 1
        );
    }
}
