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

    private bool withHelmet;

    private void Awake()
    {
    }

    void Start()
    {
        alive = true;
        body.SetActive(alive);
        corpse.SetActive(!alive);

        GetComponent<Rigidbody2D>().freezeRotation = true;

        withHelmet = gameObject.name.Contains("Helmet");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        if (!alive || !collider.CompareTag("Player")) return;

        Side side = Utils.DetectCollisionSide(collision, transform);
        Debug.Log($"Detect collision with enemy body: {side}");

        if (withHelmet || side == Side.LEFT || side == Side.RIGHT)
        {
            GlobalData.PlayerStatusData.KillPlayer();
        }
        else
        {
            Die();
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
        // if enemy die in the past, its corpse can be used in the present (removed)
        // if (transform.parent.name == "Past")
        //     transform.parent = GameObject.Find("Common").transform;

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
