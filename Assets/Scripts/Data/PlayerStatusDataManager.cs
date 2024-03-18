using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlayerStatusDataManager : IDataManager
{
    private readonly Publisher<bool> alive = new(true);

    public void Init()
    {
        alive.Update(true);
    }

    public void KillPlayer()
    {
        alive.Update(false);
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
