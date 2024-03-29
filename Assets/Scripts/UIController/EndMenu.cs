using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class EndMenu : MonoBehaviour
{
    public SendToGoogle analytics;
    public void ClickRestartFromCheckpointButton ()
    {
        GlobalData.Init();
        analytics.Send("checkpointUsed");
        SceneManager.LoadScene(GlobalData.CheckPointData.GetCurrentSceneName());
    }

    public void ClickRestartButton ()
    {
        GlobalData.Init();
        SceneManager.LoadScene(GlobalData.CheckPointData.GetCurrentSceneName());
        GlobalData.CheckPointData.ResetCheckPoint();
    }

    public void ClickMainMenuButton ()
    {   
        //Reset the stored Scene Name and the Checkpoint status
        GlobalData.CheckPointData.ResetCheckPoint();
        SceneManager.LoadScene("StartMenu");
    }
}
