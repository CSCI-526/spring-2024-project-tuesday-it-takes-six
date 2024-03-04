using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinMenu: MonoBehaviour
{
    public void ClickMainMenuButton()
    {   
        //Reset the stored Scene Name and the Checkpoint status
        GlobalData.CurrentSceneName = "";
        GlobalData.HasReachedCheckpoint = false;
        SceneManager.LoadScene("StartMenu");
    }
}
