using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject blood;

    [SerializeField]
    private SendToGoogle analytics;

    // public event EventHandler OnPlayerDied;  // leave for future in-scene game over screen
    [SerializeField]
    private Vector3 startPos = new();


    private const float MOVE_SPEED = 6;
    private const float JUMP_SPEED = 25;
    private const float GRAVITY_SCALE = 3;
    private const float FALLING_GRAVITY_SCALE = 4f;
    private const float GAME_OVER_SCENE_SHOWING_DELAY = .6f;
    private readonly Vector3 DEFAULT_START_POS = new(1.0f, 0.0f, 0.0f);


    private float horizontalInput;
    private bool jumpInput;
    
    private Subscriber<bool> playerStatusSubscriber;

    async void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;
        // start at desired position when debugging
        if (!Env.isDebug) transform.position = DEFAULT_START_POS;
        else transform.position = startPos;

        playerStatusSubscriber = GlobalData.PlayerStatusData.CreatePlayerStatusSubscriber();
        playerStatusSubscriber.Subscribe(OnPlayerDead);

        Vector3? lastPos = GlobalData.CheckPointData.GetLastCheckPointPosition();
        if (lastPos != null)
        {
            Debug.Log("Reset player to check point");
            transform.position = (Vector3)lastPos;
        }

        // Analytics initialization
        Debug.Log("pre Analytics set up!");
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics set up!");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnPlayerDead(bool alive)
    {
        if (alive) return;

        Instantiate(blood, rb.transform.position, Quaternion.identity);

        // Store the Scene Name to allow Restart to re-load the scene
        GlobalData.CheckPointData.SetCurrentSceneName(SceneManager.GetActiveScene().name);
        Invoke("LoadEndScene", GAME_OVER_SCENE_SHOWING_DELAY);
        Debug.Log($"Player Died! Player stop move! Load End scene in {GAME_OVER_SCENE_SHOWING_DELAY} seconds.");

        // Analytics
        analytics.Send("playerDied");
    }

    private void OnDestroy()
    {
        playerStatusSubscriber?.Unsubscribe(OnPlayerDead);
    }

    void Update()
    {
        // collect move control input here to avoid camera jitter
        horizontalInput = Input.GetAxis("Horizontal");
        if (!jumpInput) jumpInput = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        if (GlobalData.PlayerStatusData.IsPlayerAlive())
        {
            JumpControl();
            MoveControl();
            DeathCheck();
        }
    }

    private void MoveControl()
    {
        rb.velocity = new Vector3(horizontalInput * MOVE_SPEED, rb.velocity.y, 0);
    }

    private void JumpControl()
    {
        if (jumpInput && Utils.OnGround(rb))
        {
            rb.AddForce(Vector2.up * JUMP_SPEED, ForceMode2D.Impulse);
            jumpInput = false;
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
            GlobalData.PlayerStatusData.KillPlayer();
        }
    }


    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
