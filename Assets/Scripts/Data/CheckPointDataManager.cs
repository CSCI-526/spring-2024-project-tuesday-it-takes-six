using UnityEngine;
using Game;

public class CheckPointDataManager
{
    private readonly Publisher<Vector3?> lastCheckPointPosition = new(null);
    private readonly Publisher<bool> resetSignal = new(false);
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

    public Subscriber<bool> CreateResetSignalSubscriber()
    {
        return resetSignal.CreateSubscriber();
    }

    public void Reset()
    {
        resetSignal.Update(true);
    }
}