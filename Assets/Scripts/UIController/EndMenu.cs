using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;
using System;

public class EndMenu : MonoBehaviour
{
    public SendToGoogle analytics;

    public void ClickRestartFromCheckpointButton ()
    {
        GlobalData.Init();

        try
        {
            analytics.Send("checkpointUsed");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

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
