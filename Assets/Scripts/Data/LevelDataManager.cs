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
}
