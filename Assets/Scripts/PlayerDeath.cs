using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public event EventHandler OnPlayerDied;
    private bool OnPlayerDiedEventTriggered;

    [SerializeField] private bool died;

    private Transform playerTransform;

    private void Start()
    {
        died = false;
        OnPlayerDiedEventTriggered = false;
        playerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (playerTransform.position.y < -10.0f)
        {
            died = true;
        }

        if (died && !OnPlayerDiedEventTriggered)
        {
            OnPlayerDiedEventTriggered = true;
            Debug.Log("Player Died! Player stop move! OnPlayerDied.Invoked!");
            Rigidbody2D playerRigidbody = playerTransform.Find("PlayerRB").GetComponent<Rigidbody2D>();
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            OnPlayerDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetDeath(bool died)
    {
        this.died = died;
    }

    public bool GetDeath()
    {
        return this.died;
    }
}
