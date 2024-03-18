using System;
using UnityEngine;
using Game;

public class CheckPointDataManager : IDataManager
{
    private readonly Publisher<Vector3?> lastCheckPointPosition = new(null);
    private string currentSceneName = "";


    public void SetCurrentSceneName(string scene)
    {
        currentSceneName = scene;
    }

    public string GetCurrentSceneName()
    {
        return currentSceneName;
    }

    public void ResetCheckPoint()
    {
        currentSceneName = "";
        lastCheckPointPosition.Update(null);
    }

    public void SetLastCheckPointPosition(Vector3 pos)
    {
        lastCheckPointPosition.Update(pos);
    }

    public Vector3? GetLastCheckPointPosition()
    {
        return lastCheckPointPosition.currentValue;
    }

    public Subscriber<Vector3?> CreateLastCheckPointPositionSubscriber()
    {
        return lastCheckPointPosition.CreateSubscriber();
    }

    public void Init()
    {
        // do nothing
        // no need to reset check point information when reloading game
    }
}