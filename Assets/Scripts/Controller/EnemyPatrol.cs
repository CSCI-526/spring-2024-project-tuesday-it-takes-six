using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform patrolLeftEnd;
    public Transform patrolRightEnd;

    public float speed;

    private Rigidbody2D rigidBody;
    private bool movingRight; // if the enemy is moving to right, otherwise it is moving to left
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        ErrorChecking();

        enemyController = GetComponent<EnemyController>();

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(speed, 0);

        movingRight = true; // start with moving right since speed > 0
    }

    private void Update()
    {
        if (enemyController.IsAlive() && GlobalData.PlayerStatusData.IsPlayerAlive())
        {
            PatrolUpdate();
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
            this.enabled = false;
        }
    }

    private void ErrorChecking()
    {
        if (patrolLeftEnd == null)
        {
            Debug.LogError("Left end for enemy patrol is missing!");
        }

        if (patrolRightEnd == null)
        {
            Debug.LogError("Right end for enemy patrol is missing!");
        }

        if (patrolLeftEnd.position.x > transform.position.x)
        {
            Debug.LogError("Check left end position. Left patrol end has to be on the left of enemy default position!");
        }

        if (patrolRightEnd.position.x < transform.position.x)
        {
            Debug.LogError("Check right end position. Right patrol end has to be on the right of enemy default position");
        }
    }

    void PatrolUpdate()
    {
        // keep assigning velocity for constant move
        if (movingRight)
        {
            rigidBody.velocity = new Vector2(speed, 0);

            if (Vector2.Distance(patrolRightEnd.position, transform.position) < 0.5f)
            {
                movingRight = false;
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, 0);

            if (Vector2.Distance(patrolLeftEnd.position, transform.position) < 0.5f)
            {
                movingRight = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolLeftEnd.position, 0.5f);
        Gizmos.DrawWireSphere(patrolRightEnd.position, 0.5f);
        Gizmos.DrawLine(patrolLeftEnd.position, patrolRightEnd.position);
    }
}
