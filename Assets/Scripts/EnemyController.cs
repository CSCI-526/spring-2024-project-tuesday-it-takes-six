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
                // decide if collider(PlayerRB)'s parent has a PlayerDeath script
                if (collider.gameObject.transform.parent.TryGetComponent<PlayerDeath>(out PlayerDeath playerDeath)){
                    playerDeath.SetDeath(true);
                }
                else
                {
                    Debug.LogError("Player gameobject do not have a PlayerDeath script attached!");
                } 
            }
        }
    }

    public void Die()
    {
        alive = false;
        Destroy(body);
        // corpse is already pushable since it is an rigid body
        corpse.SetActive(true);
        // if enemy die in the past, its corpse can be used in the present
        if (transform.parent == GameObject.Find("Past").transform)
            transform.parent = GameObject.Find("Common").transform;
       
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
