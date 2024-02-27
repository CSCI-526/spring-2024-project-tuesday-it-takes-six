using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 15;
    public const float JUMP_SPEED = 50;
    public const float GRAVITY_SCALE = 10;
    public const float FALLING_GRAVITY_SCALE = 12;
    public const double EPS = 1e-4;

    public float horizontalInput;
    public Rigidbody2D rb;

    void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!GlobalData.playerDied)
        {
            MoveControl();
            JumpControl();
        }
    }

    private void MoveControl()
    {
        // deal with left/right arrows
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * MOVE_SPEED * horizontalInput);
    }

    private void JumpControl()
    {
        // jump only when player is on the ground
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) <= EPS)
        {
            rb.AddForce(Vector2.up * JUMP_SPEED, ForceMode2D.Impulse);
        }

        // make it fall faster
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = GRAVITY_SCALE;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = FALLING_GRAVITY_SCALE;
        }
    }
}
