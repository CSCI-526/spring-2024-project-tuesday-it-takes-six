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
        { TimeTense.PRESENT, new (0.0745f, 0.0824f, 0.0980f) },
        { TimeTense.PAST, new (0.3725f, 0.4118f, 0.4902f) },
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
}
