using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // used for enemy patrol
    public GameObject leftEnd;
    public GameObject rightEnd;
    private Rigidbody2D rigidBody;
    private Transform currentTarget; // current left/right point that the enemy is approaching to
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentTarget = rightEnd.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // decide which end point is moving to and change velocity
        if (currentTarget == rightEnd.transform)
        {
            rigidBody.velocity = new Vector2(speed, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, 0);
        }

        // decide if switch direction
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f
            &&
            currentTarget == rightEnd.transform)
        {
            currentTarget = leftEnd.transform;
        }
        else if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f
                  &&
                 currentTarget == leftEnd.transform)
        {
            currentTarget = rightEnd.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(leftEnd.transform.position, 0.5f);
        Gizmos.DrawWireSphere(rightEnd.transform.position, 0.5f);
        Gizmos.DrawLine(leftEnd.transform.position, rightEnd.transform.position);
    }
}
