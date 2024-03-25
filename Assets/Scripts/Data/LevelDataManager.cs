using UnityEngine.SceneManagement;

public class LevelDataManager
{
    public readonly int LEVEL_COUNT = 6;

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

    public void StartLevel(int x)
    {
        SetCurrentLevel(x);
        SceneManager.LoadScene($"Level{x}");
    }

    public bool IsLastLevel()
    {
        return currentLevel == LEVEL_COUNT;
    }
}
