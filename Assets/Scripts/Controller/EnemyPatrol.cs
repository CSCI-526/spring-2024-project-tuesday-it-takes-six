using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // used for enemy patrol
    [SerializeField]
    private float[] patrolEnds; // first value for left end, second value for right end

    public float speed;

    private Rigidbody2D rigidBody;
    private int towardLeft = -1; // current left/right point that the enemy is approaching to
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {

        if (patrolEnds[0] > transform.position.x )
        {
            Debug.LogError("Check left end position. Left patrol end has to be on the left of enemy default position!");
        }

        if (patrolEnds[1] < transform.position.x)
        {
            Debug.LogError("Check right end position. Right patrol end has to be on the right of enemy default position");
        }

        rigidBody = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
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
        rigidBody.velocity = new Vector2(speed * towardLeft, 0);
        
        // change move target
        if (Mathf.Abs(transform.position.x - patrolEnds[Mathf.Max(towardLeft, 0)]) < 0.5f)
        {
            towardLeft = 0 - towardLeft;
        }
    }


}
