using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class PlayerController : MonoBehaviour
{
    public const float MOVE_SPEED = 6;
    public const float JUMP_SPEED = 25;
    public const float GRAVITY_SCALE = 3;
    public const float FALLING_GRAVITY_SCALE = 4f;
    public const double EPS = 1e-4;

    private readonly Vector3 defaultStartPos = new(1.0f, 0.0f, 0.0f);

    public Rigidbody2D rb;

    // public event EventHandler OnPlayerDied;  // leave for future in-scene game over screen
    private bool OnPlayerDiedEventTriggered;
    // private start position, for debugging
    [SerializeField]
    private Vector3 startPos = new Vector3();

    private float horizontalInput;

    void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;
        // start at desired position when debugging
        if (!Env.isDebug) transform.position = defaultStartPos;
        else transform.position = startPos;

        GlobalData.playerDied = false;
        OnPlayerDiedEventTriggered = false;

        if(GlobalData.HasReachedCheckpoint){
            transform.position = GlobalData.LastCheckpointPosition;
        }
    }

    void Update()
    {
        // collect move control input here to avoid camera jitter
        horizontalInput = Input.GetAxis("Horizontal");

        if (!GlobalData.playerDied) JumpControl();
    }

    void FixedUpdate()
    {
        if (!GlobalData.playerDied) MoveControl();

        DeathCheck();
    }

    private void MoveControl()
    {
        rb.velocity = new Vector3(horizontalInput * MOVE_SPEED, rb.velocity.y, 0);
    }

    private void JumpControl()
    {
        bool isPlayerGrounded = Utils.OnGround(rb);

        Debug.Log($"Player grounded status: {isPlayerGrounded}");

        // jump only when player is on the ground
        if (Input.GetButtonDown("Jump") && isPlayerGrounded)
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

            // Analytics
            var eventData = new Dictionary<string, object>();
            eventData["LastCheckpointBeforeDeath"] = GlobalData.LastCheckpointPosition;
            eventData["NumberEnemiesKilledInLife"] = GlobalData.numberEnemiesKilled;
            eventData["NumberTimeSwitchesInLife"] = GlobalData.numberOfTimeSwitches;

            Analytics.CustomEvent("PlayerDied", eventData);

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
