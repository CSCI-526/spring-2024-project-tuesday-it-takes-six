using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 15;
    public const float JUMP_SPEED = 50;
    public const float GRAVITY_SCALE = 10;
    public const float FALLING_GRAVITY_SCALE = 12;
    public const double EPS = 1e-4;

    public float horizontalInput;
    public Rigidbody2D rb;

    // public event EventHandler OnPlayerDied;  // leave for future in-scene game over screen
    private bool OnPlayerDiedEventTriggered;

    void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;

        GlobalData.playerDied = false;
        OnPlayerDiedEventTriggered = false;
    }

    void Update()
    {
        if (!GlobalData.playerDied)
        {
            MoveControl();
            JumpControl();
        }

        DeathCheck();
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

    private void DeathCheck()
    {
        if (rb.position.y < -10.0f)
        {
            GlobalData.playerDied = true;
        }

        if (GlobalData.playerDied && !OnPlayerDiedEventTriggered)
        {
            OnPlayerDiedEventTriggered = true;
            Invoke("LoadEndScene", 1.5f);
            Debug.Log("Player Died! Player stop move! Load End scene in 1.5 seconds.");

            // OnPlayerDied?.Invoke(this, EventArgs.Empty);  // leave for future in-game game over screen
        }
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void SetDeath(bool died)
    {
        GlobalData.playerDied = died;
    }

    public bool GetDeath()
    {
        return GlobalData.playerDied;
    }
}
