using UnityEngine;
using Game;
using System;

public class MovingFloorController : MonoBehaviour
{
    [SerializeField]
    private Axis movingAxis;

    [SerializeField]
    [Range(0, 20)]
    private float movingRange = 3;

    [SerializeField]
    [Range(0, 2f)]
    private float speed = 1f;

    [SerializeField]
    [Range(0, 40)]
    private float initialPositionOffset = 0;

    private bool direction; // true: start -> end, false: end -> start

    private Vector3 startPosition;
    private Vector3 endPosition;

    public void Start()
    {
        if (initialPositionOffset > movingRange * 2)
        {
            throw new Exception("initial position offset should be less than 2 * moving range");
        }

        Vector3 range = movingAxis == Axis.HORIZONTAL
            ? new Vector3(movingRange, 0, 0)
            : new Vector3(0, movingRange, 0);

        Vector3 initOffset = movingAxis == Axis.HORIZONTAL
            ? new Vector3(initialPositionOffset, 0, 0)
            : new Vector3(0, initialPositionOffset, 0);
        
        startPosition = transform.position - range;
        endPosition = transform.position + range;

        transform.position = startPosition + initOffset;
        direction = Vector3.Distance(endPosition, transform.position) > 0.01f;
    }

    public void FixedUpdate()
    {
        Vector3 directionVector;
        if (movingAxis == Axis.HORIZONTAL) directionVector = direction ? Vector3.right : Vector3.left;
        else directionVector = direction ? Vector3.up : Vector3.down;

        transform.Translate(directionVector * Time.deltaTime * speed);

        if ((!direction && Vector3.Distance(startPosition, transform.position) < 0.01f)
            || (direction && Vector3.Distance(endPosition, transform.position) < 0.01f))
        {
            direction = !direction;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.transform.parent;
        player.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.transform.parent;
        player.transform.SetParent(null);
    }
}
