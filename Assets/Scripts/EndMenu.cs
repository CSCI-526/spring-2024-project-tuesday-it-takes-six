using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void ClickRestartButton ()
    {
        // TODO: read last checkpoint, reload from there
        SceneManager.LoadScene("Tutorial");
    }

    public void ClickMainMenuButton ()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
