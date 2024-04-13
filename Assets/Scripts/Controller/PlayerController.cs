using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject blood;

    [SerializeField]
    private SendToGoogle analytics;

    [SerializeField]
    private Vector3 startPos = new();


    private const float MOVE_SPEED = 5;
    private const float JUMP_SPEED = 20;
    private const float GRAVITY_SCALE = 2;
    private const float FALLING_GRAVITY_SCALE = 2.2f;
    private readonly Vector3 DEFAULT_START_POS = new(1.0f, 0.0f, 0.0f);
    private const float GAME_OVER_SCENE_SHOWING_DELAY = .6f;


    private float horizontalInput;
    private bool jumpInput;
    
    private Subscriber<bool> playerStatusSubscriber;
    private Subscriber<bool> resetSubSubscriber;

    async void Start()
    {
        // prevent it from rotating when hitting other objects
        rb.freezeRotation = true;
        // start at desired position when debugging
        if (!Env.isDebug) transform.position = DEFAULT_START_POS;
        else transform.position = startPos;

        // Analytics initialization
        GlobalData.AnalyticsManager = analytics;

        playerStatusSubscriber = GlobalData.PlayerStatusData.CreatePlayerStatusSubscriber();
        playerStatusSubscriber.Subscribe(OnPlayerDead);

        resetSubSubscriber = GlobalData.CheckPointData.CreateResetSignalSubscriber();
        resetSubSubscriber.Subscribe(OnReset);

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

    private void OnDestroy()
    {
        playerStatusSubscriber?.Unsubscribe(OnPlayerDead);
        resetSubSubscriber?.Unsubscribe(OnReset);
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
        GlobalData.AnalyticsManager.Send("playerDied");
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
        GlobalData.OverlayData.ShowGameOver();
    }

    private void OnReset(bool _)
    {
        Vector3 lastPos = GlobalData.CheckPointData.GetLastCheckPointPosition() ?? DEFAULT_START_POS;
        transform.position = lastPos;
        rb.transform.localPosition = Vector3.zero;
        GlobalData.PlayerStatusData.RevivePlayer();
        Debug.Log($"Reset player to check point {lastPos}");

        try
        {
            GlobalData.AnalyticsManager.Send("checkpointUsed");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
