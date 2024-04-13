using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;
using System;

public class EndMenu : MonoBehaviour
{
    

    public void ClickRestartFromCheckpointButton ()
    {
        GlobalData.Init();

        SceneManager.LoadScene(GlobalData.CheckPointData.GetCurrentSceneName());
    }

    public void ClickRestartButton ()
    {
    }

    public void ClickMainMenuButton ()
    {   
        //Reset the stored Scene Name and the Checkpoint status
    }
}
