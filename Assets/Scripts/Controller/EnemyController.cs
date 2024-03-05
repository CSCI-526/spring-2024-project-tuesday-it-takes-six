using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.UIElements;
using UnityEngine.Analytics;

public class EnemyController : MonoBehaviour
{
    private bool alive;

    [SerializeField]
    private GameObject body;

    [SerializeField]
    private GameObject corpse;

    // time portal that may interat with the enemy
    [SerializeField] private GameObject timePortal;

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
                    Debug.LogError("collider's parent do not have a PlayerController script attached!");
                }
            }
        }
    }

    private void Die()
    {
        // if enemy already died, no need to die again
        if (!alive)
        {
            Debug.LogWarning("Enemy.Die() not triggered because enemy already died");
        }

        alive = false;
        body.SetActive(alive);
        corpse.SetActive(!alive);
        // Enable rotation
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        // corpse is already pushable since it is an rigid body
        // if enemy die in the past, its corpse can be used in the present
        if (transform.parent.name == "Past")
            transform.parent = GameObject.Find("Common").transform;

        // Analytics
        GlobalData.numberEnemiesKilled += 1;

    }

    public bool IsAlive()
    {
        return alive;
    }

    public TimeTense? GetTimeTense()
    {
        if (transform.parent.name == "Past") return TimeTense.PAST;
        if (transform.parent.name == "Present") return TimeTense.PRESENT;
        return null;
    }
}
