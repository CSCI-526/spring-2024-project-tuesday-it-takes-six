using UnityEngine;
using Game;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverDialog;

    private Subscriber<OverlayContent> OverlayContentSubscriber;

    private void Start()
    {
        gameObject.SetActive(false);

        OverlayContentSubscriber = GlobalData.OverlayData.CreateLastCheckPointPositionSubscriber();
        OverlayContentSubscriber.Subscribe(OnOverlayContentChange);
    }

    private void OnDestroy()
    {
        OverlayContentSubscriber?.Unsubscribe(OnOverlayContentChange);
    }

    private void OnOverlayContentChange(OverlayContent t)
    {
        Debug.Log($"[OverlayController] Status Change to {t}");
        if (t == OverlayContent.NONE)
        {
            gameObject.SetActive(false);
        }
        else if (t == OverlayContent.GAME_OVER)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        GameOverDialog.SetActive(true);
        gameObject.SetActive(true);
    }


    public void RestartFromCheckpoint()
    {
        GlobalData.CheckPointData.Reset();
        GlobalData.OverlayData.HideOverlay();
    }

    public void RestartLevel()
    {
        GlobalData.Init();
        GlobalData.LevelData.RestartCurrentLevel();
        GlobalData.CheckPointData.ResetCheckPoint();
    }

    public void BackToMainMenu()
    {
        GlobalData.CheckPointData.ResetCheckPoint();
        SceneManager.LoadScene("StartMenu");
    }
}
