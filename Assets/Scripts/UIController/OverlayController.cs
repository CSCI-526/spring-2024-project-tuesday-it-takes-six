using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverDialog;

    [SerializeField]
    private GameObject InGameMenuDialog;


    private Subscriber<OverlayContent> OverlayContentSubscriber;
    private GameObject HUD;


    private void Start()
    {
        HUD = GameObject.Find("HUD");

        OverlayContentSubscriber = GlobalData.OverlayData.CreateLastCheckPointPositionSubscriber();
        OverlayContentSubscriber.Subscribe(OnOverlayContentChange);
    }

    private void OnDestroy()
    {
        OverlayContentSubscriber?.Unsubscribe(OnOverlayContentChange);
    }

    private void OnOverlayContentChange(OverlayContent t)
    {
        Time.timeScale = t == OverlayContent.NONE ? 1 : 0;

        HUD.SetActive(t == OverlayContent.NONE);
        UpdateOverlays(t);
    }

    private void UpdateOverlays(OverlayContent t)
    {
        GameOverDialog.SetActive(t == OverlayContent.GAME_OVER);
        InGameMenuDialog.SetActive(t == OverlayContent.IN_GAME_MENU);
    }

    public void Restart()
    {
        GlobalData.CheckPointData.Reset();
        GlobalData.OverlayData.HideOverlay();
    }

    public void Reload()
    {
        GlobalData.Init();
        GlobalData.LevelData.RestartCurrentLevel();

        GlobalData.OverlayData.HideOverlay();
    }

    public void BackToMainMenu()
    {
        GlobalData.CheckPointData.ResetCheckPoint();
        SceneManager.LoadScene("StartMenu");

        GlobalData.OverlayData.HideOverlay();
    }
}
