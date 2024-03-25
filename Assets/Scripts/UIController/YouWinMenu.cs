using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinMenu: MonoBehaviour
{
    [SerializeField]
    private GameObject LevelFinishDisplay;
    [SerializeField]
    private GameObject AllFinishDisplay;

    private bool keyPressed = false;

    private void Start()
    {
        bool isLastLevel = GlobalData.LevelData.IsLastLevel();
        LevelFinishDisplay.SetActive(!isLastLevel);
        AllFinishDisplay.SetActive(isLastLevel);
    }

    private void Update()
    {
        if (Input.anyKey && !keyPressed)
        {
            keyPressed = true;
        }

        if (keyPressed)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (GlobalData.LevelData.IsLastLevel()) return;

        int nextLevel = GlobalData.LevelData.GetNextLevel();
        GlobalData.LevelData.StartLevel(nextLevel);
    }

    public void ClickMainMenuButton()
    {   
        //Reset the stored Scene Name and the Checkpoint status
        GlobalData.CheckPointData.ResetCheckPoint();
        SceneManager.LoadScene("StartMenu");
    }
}
