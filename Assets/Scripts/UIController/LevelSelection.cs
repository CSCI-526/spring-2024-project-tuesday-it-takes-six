using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void StartLevel(int x)
    {
        GlobalData.LevelData.SetCurrentLevel(x);
        SceneManager.LoadScene($"Level{x}");
    }

    public void LoadNextLevel()
    {
        int nextLevel = GlobalData.LevelData.GetNextLevel();
        StartLevel(nextLevel);
    }
}
