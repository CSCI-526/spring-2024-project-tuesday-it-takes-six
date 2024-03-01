using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // used for enemy patrol
    public GameObject leftEnd;
    public GameObject rightEnd;
    public float speed;

    private Rigidbody2D rigidBody;
    private Transform currentTarget; // current left/right point that the enemy is approaching to
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        if (leftEnd == null)
        {
            Debug.LogError("Left end for enemy patrol is missing!");
        }

        if (rightEnd == null)
        {
            Debug.LogError("Right end for enemy patrol is missing!");
        }

        if (leftEnd.transform.position.x > transform.position.x )
        {
            Debug.LogError("Check left end position. Left patrol end has to be on the left of enemy default position!");
        }

        if (rightEnd.transform.position.x < transform.position.x)
        {
            Debug.LogError("Check right end position. Right patrol end has to be on the right of enemy default position");
        }

        rigidBody = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();

        currentTarget = rightEnd.transform;
        rigidBody.velocity = new Vector2(speed, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyController.IsAlive())
        {
            PatrolUpdate();
        }
        else
        {
            this.enabled = false;
        }
    }

    void PatrolUpdate()
    {
        // keep assigning velocity for constant move
        if (currentTarget == rightEnd.transform)
        {
            rigidBody.velocity = new Vector2(speed, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, 0);

        }

        // change move target
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            if (currentTarget == leftEnd.transform)
            {
                currentTarget = rightEnd.transform;
            }
            else
            {
                currentTarget = leftEnd.transform;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(leftEnd.transform.position, 0.5f);
        Gizmos.DrawWireSphere(rightEnd.transform.position, 0.5f);
        Gizmos.DrawLine(leftEnd.transform.position, rightEnd.transform.position);
    }
}
