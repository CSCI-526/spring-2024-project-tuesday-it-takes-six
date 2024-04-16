using System;
using UnityEngine;
using Game;

public class PlayerStatusDataManager : IDataManager
{
    private readonly Publisher<bool> alive = new(true);

    private readonly int IMMORTAL_TIME = 500; // in milliseconds
    private DateTime respawnTime = DateTime.Now;

    public void Init()
    {
        Respawn();
    }

    public void KillPlayer()
    {
        int gap = (DateTime.Now - respawnTime).Milliseconds;
        if (alive.currentValue && gap <= IMMORTAL_TIME)
        {
            Debug.Log($"Player is immortal from respawning {gap} milliseconds ago");
            return;
        }

        if (alive.currentValue)
        {
            alive.Update(false);
        }
    }

    public void Respawn()
    {
        alive.Update(true);
        respawnTime = DateTime.Now;
    }

    public bool IsPlayerAlive()
    {
        return alive.currentValue;
    }

    public Subscriber<bool> CreatePlayerStatusSubscriber()
    {
        return alive.CreateSubscriber();
    }
}
