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
        body.SetActive(alive);
        corpse.SetActive(!alive);

        GetComponent<Rigidbody2D>().freezeRotation = true;
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
            Debug.Log($"Collide on side: {side}");

            // jump on top will kill the enemy
            if (side == "Top")
            {
                Die();
            }
            // otherwise the player die
            else
            {
                // check if collider(PlayerRB)'s parent has a PlayerController script
                if (collider.transform.parent.TryGetComponent<PlayerController>(out PlayerController playerController))
                {
                    playerController.SetDeath(true);
                }
                else
                {
                    Debug.LogError("Player gameobject do not have a PlayerController script attached!");
                }
            }
        }
    }

    protected bool Die()
    {
        // if enemy already died, no need to die again
        if (!alive)
        {
            Debug.LogWarning("Enemy.Die() not triggered because enemy already died");
            return false;
        }

        alive = false;
        body.SetActive(alive);
        corpse.SetActive(!alive);

        // corpse is already pushable since it is an rigid body
        // if enemy die in the past, its corpse can be used in the present
        if (transform.parent == GameObject.Find("Past").transform)
            transform.parent = GameObject.Find("Common").transform;

        return true;
    }

}
