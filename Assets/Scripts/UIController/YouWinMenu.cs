using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinMenu: MonoBehaviour
{
    [SerializeField]
    private GameObject LevelFinishDisplay;
    [SerializeField]
    private GameObject AllFinishDisplay;


    private void Start()
    {
        bool isLastLevel = GlobalData.LevelData.IsLastLevel();
        LevelFinishDisplay.SetActive(!isLastLevel);
        AllFinishDisplay.SetActive(isLastLevel);
    }

    public void LoadNextLevel()
    {
        if (GlobalData.LevelData.IsLastLevel()) return;

        int nextLevel = GlobalData.LevelData.GetNextLevel();

        GlobalData.Init();
        GlobalData.CheckPointData.ResetCheckPoint();
        var sceneName = GlobalData.LevelData.StartLevel(nextLevel);
        GlobalData.CheckPointData.SetCurrentSceneName(sceneName);
    }

    public void ClickMainMenuButton()
    {   
        // Reset the stored Scene Name and the Checkpoint status
        GlobalData.CheckPointData.ResetCheckPoint();
        SceneManager.LoadScene("StartMenu");
    }
}
