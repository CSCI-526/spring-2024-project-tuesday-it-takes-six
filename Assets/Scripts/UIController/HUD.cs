using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeTense;

    [SerializeField]
    private TMP_Text level;

    private Subscriber<TimeTense> subscriber;

    private readonly Dictionary<TimeTense, string> TEXT_MAPPING = new()
    {
        { TimeTense.PRESENT, "Time: Present" },
        { TimeTense.PAST, "Time: Past" },
    };

    private readonly Dictionary<TimeTense, Color> COLOR_MAPPING = new()
    {
        { TimeTense.PAST, new (0.249f, 0.260f, 0.2735f) },
        { TimeTense.PRESENT, new (0.219f, 0.2809f, 0.304f) },
    };


    // Start is called before the first frame update
    private void Start()
    {
        level.text = SceneManager.GetActiveScene().name;

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);
    }

    private void OnDestroy()
    {
        subscriber?.Unsubscribe(OnTimeSwitch);
    }

    private void OnTimeSwitch(TimeTense tt)
    {
        timeTense.text = TEXT_MAPPING[tt];
        Camera.main.backgroundColor = COLOR_MAPPING[tt];
    }

    private void Update()
    {
        var current = GlobalData.OverlayData.GetActiveOverlay();
        if (current == OverlayContent.GAME_OVER) return;

        if (current == OverlayContent.NONE && Input.GetButtonDown("Pause"))
        {
            GlobalData.OverlayData.ShowInGameMenu();
        }
        else if (current == OverlayContent.IN_GAME_MENU && Input.GetButtonUp("Pause"))
        {
            GlobalData.OverlayData.HideOverlay();
        }
    }
}
