using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class EndMenu : MonoBehaviour
{
    public void ClickRestartFromCheckpointButton ()
    {
        //Reload the playing scene
        SceneManager.LoadScene(GlobalData.CurrentSceneName);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //If the player reached a checkpoint before, restart the level with player at the latest checkpoint
        if (GlobalData.HasReachedCheckpoint && player != null)
        {
            player.transform.position = GlobalData.LastCheckpointPosition;
        }
        else
        {
            Debug.LogWarning("Player not found in scene.");
        }

    }

    public void ClickRestartButton ()
    {
        //Reset the HasReachedCheckpoint value for the current playing scene
        GlobalData.HasReachedCheckpoint = false;

        SceneManager.LoadScene(GlobalData.CurrentSceneName);
    }

    public void ClickMainMenuButton ()
    {   
        //Reset the stored Scene Name and the Checkpoint status
        GlobalData.CurrentSceneName = "";
        GlobalData.HasReachedCheckpoint = false;
        SceneManager.LoadScene("StartMenu");
    }
}
