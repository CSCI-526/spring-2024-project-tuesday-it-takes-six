using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class MovingFloorController : MonoBehaviour
{
    [SerializeField]
    private Axis movingAxis;

    [SerializeField]
    [Range(0, 20)]
    private float movingRange = 3;

    [SerializeField]
    [Range(0, 5)]
    private float speed = 2;

    private bool direction; // true: start -> end, false: end -> start

    private Vector3 startPosition;
    private Vector3 endPosition;

    public void Start()
    {
        Vector3 offset = movingAxis == Axis.HORIZONTAL
            ? new Vector3(movingRange, 0, 0)
            : new Vector3(0, movingRange, 0);
        
        startPosition = transform.position - offset;
        endPosition = transform.position + offset;

        transform.position = startPosition;
        direction = true;
    }

    public void Update()
    {
        Vector3 directionVector;
        if (movingAxis == Axis.HORIZONTAL) directionVector = direction ? Vector3.right : Vector3.left;
        else directionVector = direction ? Vector3.up : Vector3.down;

        transform.Translate(directionVector * Time.deltaTime * speed);
        Debug.Log(Vector3.Distance(endPosition, transform.position));

        if ((!direction && Vector3.Distance(startPosition, transform.position) < 0.5f)
            || (direction && Vector3.Distance(endPosition, transform.position) < 0.5f))
        {
            direction = !direction;
        }
    }
}
