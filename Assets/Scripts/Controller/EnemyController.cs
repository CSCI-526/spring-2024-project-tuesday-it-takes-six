using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class EnemyController : MonoBehaviour
{
    private bool alive;

    public GameObject body;
    public GameObject corpse;

    void Start()
    {
        alive = true;
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
            string side = Utils.DetectCollisionSide(gameObject, collider);

            // jump on top will kill the enemy
            if (side == "Top")
            {
                Die();
            }
            // otherwise the player die
            else
            {
                // decide if collider(PlayerRB)'s parent has a PlayerDeath script
                if (collider.gameObject.transform.parent.TryGetComponent<PlayerDeath>(out PlayerDeath playerDeath))
                {
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
        // Enemy already died, no need to die again
        if (!alive) return;

        alive = false;
        Destroy(body);

        // corpse is already pushable since it is an rigid body
        corpse.SetActive(true);
        // if enemy die in the past, its corpse can be used in the present
        Debug.Log(transform.parent);
        if (transform.parent == GameObject.Find("Past").transform)
            transform.parent = GameObject.Find("Common").transform;
    }

}
