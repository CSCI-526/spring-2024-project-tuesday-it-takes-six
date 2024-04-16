using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class CheckPointDataManager
{
    private readonly Publisher<Vector3?> lastCheckPointPosition = new(null);
    private readonly Publisher<bool> resetSignal = new(false);

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void ResetCheckPoint()
    {
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