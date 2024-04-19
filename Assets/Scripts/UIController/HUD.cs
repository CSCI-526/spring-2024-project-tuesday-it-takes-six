using System;
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
    private Subscriber<OverlayContent> overlayContentSubscriber;


    private readonly Dictionary<TimeTense, string> TEXT_MAPPING = new()
    {
        { TimeTense.PRESENT, "Time: Present" },
        { TimeTense.PAST, "Time: Past" },
    };

    private readonly Dictionary<TimeTense, Color> COLOR_MAPPING = new()
    {
        { TimeTense.PAST, new (0.83529412f, 0.64313725f, 0.38431373f) },
        { TimeTense.PRESENT, new (0.219f, 0.2809f, 0.304f) },
    };


    // Start is called before the first frame update
    private void Start()
    {
        level.text = SceneManager.GetActiveScene().name;

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);

        overlayContentSubscriber = GlobalData.OverlayData.CreateLastCheckPointPositionSubscriber();
        overlayContentSubscriber.Subscribe(OnOverlayContentChange);
    }

    private void OnDestroy()
    {
        subscriber?.Unsubscribe(OnTimeSwitch);
        overlayContentSubscriber?.Unsubscribe(OnOverlayContentChange);
    }

    private void OnOverlayContentChange(OverlayContent t)
    {
        gameObject.SetActive(t == OverlayContent.NONE);
    }

    private void OnTimeSwitch(TimeTense tt)
    {
        timeTense.text = TEXT_MAPPING[tt];
        Camera.main.backgroundColor = COLOR_MAPPING[tt];
    }
}
