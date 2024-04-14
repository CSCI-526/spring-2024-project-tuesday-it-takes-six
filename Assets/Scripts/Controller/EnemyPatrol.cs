using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : ResetableMonoBehaviour
{
    [SerializeField]
    private bool enablePatrol = true;

    [SerializeField]
    [Range(-10, 0)]
    private float patrolLeftRange = -3;

    [SerializeField]
    [Range(0, 10)]
    private float patrolRightRange = 3;

    [SerializeField]
    private float speed = 1;


    private Rigidbody2D rigidBody;
    private bool movingRight; // if the enemy is moving to right, otherwise it is moving to left
    private EnemyController enemyController;

    private Vector3 patrolLeftEnd;
    private Vector3 patrolRightEnd;

    // Start is called before the first frame update
    override public void Start()
    {
        // ErrorChecking();
        base.Start();
        enemyController = GetComponent<EnemyController>();

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(speed, 0);

        movingRight = true; // start with moving right since speed > 0

        patrolLeftEnd = transform.position + new Vector3(patrolLeftRange, 0, 0);
        patrolRightEnd = transform.position + new Vector3(patrolRightRange, 0, 0);
    }

    private void Update()
    {
        if (enablePatrol && enemyController.IsAlive() && GlobalData.PlayerStatusData.IsPlayerAlive())
        {
            PatrolUpdate();
        }
        else
        {
            enabled = false;
            rigidBody.velocity = new Vector2(0, 0);
        }
    }

    // private void ErrorChecking()
    // {
    //     if (patrolLeftEnd == null)
    //     {
    //         Debug.LogError("Left end for enemy patrol is missing!");
    //     }

    //     if (patrolRightEnd == null)
    //     {
    //         Debug.LogError("Right end for enemy patrol is missing!");
    //     }

    //     if (patrolLeftEnd.position.x > transform.position.x)
    //     {
    //         Debug.LogError("Check left end position. Left patrol end has to be on the left of enemy default position!");
    //     }

    //     if (patrolRightEnd.position.x < transform.position.x)
    //     {
    //         Debug.LogError("Check right end position. Right patrol end has to be on the right of enemy default position");
    //     }
    // }

    void PatrolUpdate()
    {
        // keep assigning velocity for constant move
        if (movingRight)
        {
            rigidBody.velocity = new Vector2(speed, 0);

            if (Vector2.Distance(patrolRightEnd, transform.position) < 0.5f)
            {
                movingRight = false;
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, 0);

            if (Vector2.Distance(patrolLeftEnd, transform.position) < 0.5f)
            {
                movingRight = true;
            }
        }
    }


    override public void OnReset(bool _)
    {
        if (enablePatrol) enabled = true;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(patrolLeftEnd.position, 0.5f);
    //     Gizmos.DrawWireSphere(patrolRightEnd.position, 0.5f);
    //     Gizmos.DrawLine(patrolLeftEnd.position, patrolRightEnd.position);
    // }
}
