using System.Numerics;
using UnityEngine.SceneManagement;

public class LevelDataManager
{
    public readonly int LEVEL_COUNT = 5;

    private int currentLevel = 1;
    private int maxLevelReached = 1;


    public void SetCurrentLevel(int x)
    {
        currentLevel = x;
        if (maxLevelReached < currentLevel) maxLevelReached = currentLevel;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetNextLevel()
    {
        return currentLevel + 1;
    }

    public int GetMaxLevelReached()
    {
        return maxLevelReached;
    }

    public string StartLevel(int x)
    {
        SetCurrentLevel(x);
        string sceneName = $"Level{x}";
        SceneManager.LoadScene(sceneName);
        return sceneName;
    }

    public bool IsLastLevel()
    {
        return currentLevel == LEVEL_COUNT;
    }

    public void RestartCurrentLevel()
    {
        StartLevel(currentLevel);
    }
}
