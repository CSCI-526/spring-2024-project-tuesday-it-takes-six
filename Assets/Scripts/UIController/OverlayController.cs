using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverDialog;

    [SerializeField]
    private GameObject InGameMenuDialog;


    private Subscriber<OverlayContent> overlayContentSubscriber;


    private void Start()
    {
        overlayContentSubscriber = GlobalData.OverlayData.CreateLastCheckPointPositionSubscriber();
        overlayContentSubscriber.Subscribe(OnOverlayContentChange);
    }

    private void OnDestroy()
    {
        overlayContentSubscriber?.Unsubscribe(OnOverlayContentChange);
    }

    private void OnOverlayContentChange(OverlayContent t)
    {
        Time.timeScale = t == OverlayContent.NONE ? 1 : 0;

        UpdateOverlays(t);
    }

    private void UpdateOverlays(OverlayContent t)
    {
        GameOverDialog.SetActive(t == OverlayContent.GAME_OVER);
        InGameMenuDialog.SetActive(t == OverlayContent.IN_GAME_MENU);
    }

    public void Respawn()
    {
        GlobalData.CheckPointData.Reset();
        GlobalData.OverlayData.HideOverlay();
    }

    public void ResetWorld()
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


    private void Update()
    {
        var current = GlobalData.OverlayData.GetActiveOverlay();

        if (current == OverlayContent.GAME_OVER) return;

        if (current == OverlayContent.NONE && Input.GetButtonDown("Pause"))
        {
            GlobalData.OverlayData.ShowInGameMenu();
        }
        else if (current == OverlayContent.IN_GAME_MENU && Input.GetButtonDown("Pause"))
        {
            GlobalData.OverlayData.HideOverlay();
        }
    }
}
