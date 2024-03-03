using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 8;
    public const float JUMP_SPEED = 45;
    public const float GRAVITY_SCALE = 8;
    public const float FALLING_GRAVITY_SCALE = 12;
    public const double EPS = 1e-4;
    private Vector3 defaultStartPos = new(0.253f, 0.011f, 0.0f);

    public float horizontalInput;
    public Rigidbody2D rb;

    // public event EventHandler OnPlayerDied;  // leave for future in-scene game over screen
    private bool OnPlayerDiedEventTriggered;
    // private start position, for debugging
    [SerializeField]
    private Vector3 startPos = new Vector3();
    // if the we are going to debug
    [SerializeField]
    private bool isDebug = false;
    void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;
        // start at desired position when debugging
        if (!isDebug) transform.position = defaultStartPos;
        else transform.position = startPos;

        GlobalData.playerDied = false;
        OnPlayerDiedEventTriggered = false;

        if(GlobalData.HasReachedCheckpoint){
            transform.position = GlobalData.LastCheckpointPosition;
        }
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
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(horizontalInput * MOVE_SPEED, rb.velocity.y, 0);
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
            //Store the Scene Name to allow Restart to re-load the scene
            GlobalData.CurrentSceneName = SceneManager.GetActiveScene().name;
            Invoke("LoadEndScene", 0.6f);
            Debug.Log("Player Died! Player stop move! Load End scene in 0.6 seconds.");

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
