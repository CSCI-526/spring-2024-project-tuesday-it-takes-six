using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Game;


public class TimeSwitch : MonoBehaviour
{
    // private static readonly Dictionary<TimeTense, string> ChangeableCalls = new Dictionary<TimeTense, string>()
    // {
    //     { TimeTense.PRESENT, "OnPresent" },
    //     { TimeTense.PAST, "OnPast" }
    // };

    [SerializeField]
    private GameObject presentObjects;
    [SerializeField]
    private GameObject pastObjects;

    /// <summary>
    /// ⚠️ DEPRECATED
    /// </summary>
    [SerializeField]
    private GameObject ChangeableObjects;

    private GameObject player;

    public SendToGoogle analytics;

    private Subscriber<TimeTense> subscriber;

    private void Start()
    {
        player = GameObject.Find("Player");

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);
    }

    private void OnDestroy()
    {
        subscriber?.Unsubscribe(OnTimeSwitch);
    }

    private void Update()
    {
        if (Input.GetButtonDown("TimeSwitch"))
        {
            GlobalData.TimeTenseData.SwitchTimeTense();
        }
    }

    private void OnTimeSwitch(TimeTense tt)
    {
        Debug.Log("Time Switched listened");
        // update object groups
        UpdatePastObjects(tt);
        UpdatePresentObjects(tt);

        // Reset Player
        player.transform.SetParent(null);

        if (Env.isDebug) return;

        // Analytics
        GlobalData.numberOfTimeSwitches += 1;
        analytics.Send();
        var eventData = new Dictionary<string, object>();
        eventData["LastCheckpointBeforeTimeSwitch"] = GlobalData.CheckPointData.GetLastCheckPointPosition();

        Analytics.CustomEvent("TimeSwitched", eventData);
        Analytics.FlushEvents();
        Debug.Log("Analytics time switch event submitted");
    }


    private void UpdatePresentObjects(TimeTense tt)
    {
        foreach (Transform presentTransform in presentObjects.transform)
        {
            GameObject presentObj = presentTransform.gameObject;
            presentObj.SetActive(tt == TimeTense.PRESENT);
        }
    }

    private void UpdatePastObjects(TimeTense tt)
    {
        foreach (Transform pastTransform in pastObjects.transform)
        {
            GameObject pastObj = pastTransform.gameObject;
            pastObj.SetActive(tt == TimeTense.PAST);
        }
    }
}
