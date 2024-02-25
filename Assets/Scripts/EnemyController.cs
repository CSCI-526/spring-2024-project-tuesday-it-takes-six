using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool alive = true;

    public GameObject body;
    public GameObject corpse;

    void Start()
    {
        corpse.SetActive(false);
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Detect collision with enemy body");
        Collider2D collider = collision.collider;

        if (alive && collider.CompareTag("Player"))
        {
            string side = DetectCollisionSide(collider);

            // jump on top will kill the enemy
            if (side == "Top")
            {
                Die();
            }
            // otherwise the player die
            else
            {
                // TODO: player will die, show the finish scene
                Debug.Log("Player died");
            }
        }
    }

    private void Die()
    {
        alive = false;
        Destroy(body);

        corpse.SetActive(true);

        // TODO: resize collider box to the size of the corpse
        
        // TODO: make corpse push-able
    }

    private string DetectCollisionSide(Collider2D collider)
    {
        Vector3 dir = (collider.gameObject.transform.position - gameObject.transform.position).normalized;
        var angle = Vector2.SignedAngle(transform.right, dir);

        if (Mathf.Abs(angle) <= 45) return "Right";
        if (angle > 45 && angle <= 135) return "Top";
        if (angle < 45 && angle >= -135) return "Bottom";
        return "Left";
    }
}
