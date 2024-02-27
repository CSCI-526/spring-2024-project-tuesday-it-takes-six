using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class PlayerDeath : MonoBehaviour
{
    // public event EventHandler OnPlayerDied;  // leave for future in-scene game over screen
    private bool OnPlayerDiedEventTriggered;

    [SerializeField] private Transform playerRBTransform; // Player rigid body, not Player 

    private void Start()
    {
        GlobalData.playerDied = false;
        OnPlayerDiedEventTriggered = false;
    }

    private void Update()
    {
        if (playerRBTransform.position.y < -10.0f)
        {
            GlobalData.playerDied = true;
        }

        if (GlobalData.playerDied && !OnPlayerDiedEventTriggered)
        {
            OnPlayerDiedEventTriggered = true;
            Debug.Log("Player Died! Player stop move! Load End scene in 2 seconds");
            Invoke("LoadEndScene", 2.0f);

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
