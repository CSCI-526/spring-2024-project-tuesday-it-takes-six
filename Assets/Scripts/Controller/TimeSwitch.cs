using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;


public class TimeSwitch : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    private static readonly Dictionary<TimeTense, string> ChangeableCalls = new Dictionary<TimeTense, string>()
    {
        { TimeTense.PRESENT, "OnPresent" },
        { TimeTense.PAST, "OnPast" }
    };

    [SerializeField]
    private GameObject presentObjects;
    [SerializeField]
    private GameObject pastObjects;
    [SerializeField]
    private GameObject ChangeableObjects;

    public SendToGoogle analytics;

    void Start()
    {
        GlobalData.TimeTenseData.Init();
        // TODO(keyi): refactor using pub-sub
        // OnTimeSwitch(reportToAnalytics: false); // no need to do time switch on start, some components haven't been initailized
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeSwitch"))
        {
            Switch();
        }
    }

    private void UpdatePresentObjects()
    {
        foreach (Transform presentTransform in presentObjects.transform)
        {
            GameObject presentObj = presentTransform.gameObject;
            presentObj.SetActive(GlobalData.TimeTenseData.IsPresent());
        }
    }

    private void UpdatePastObjects()
    {
        foreach (Transform pastTransform in pastObjects.transform)
        {
            GameObject pastObj = pastTransform.gameObject;
            pastObj.SetActive(GlobalData.TimeTenseData.IsPast());
        }
    }

    private void UpdateChangeableObjects()
    {
        ChangeableObjects.BroadcastMessage(
            ChangeableCalls[GlobalData.TimeTenseData.GetTimeTense()],
            SendMessageOptions.DontRequireReceiver
        );
    }

    private void UpdateUI()
    {
        label.text = GlobalData.TimeTenseData.GetDisplayText();
        Camera.main.backgroundColor = GlobalData.TimeTenseData.GetBackgroundColor();
    }


    /// <summary>
    /// Will be triggered when time is switched
    /// </summary>
    private void OnTimeSwitch(bool reportToAnalytics = true)
    {
        // update object groups
        UpdatePastObjects();
        UpdatePresentObjects();
        UpdateChangeableObjects();

        // update text and background color 
        UpdateUI();

        if (!reportToAnalytics || Env.isDebug) return;

        // Analytics
        GlobalData.numberOfTimeSwitches += 1;
        analytics.Send();
        var eventData = new Dictionary<string, object>();
        eventData["LastCheckpointBeforeTimeSwitch"] = GlobalData.CheckPointData.GetLastCheckPointPosition();

        Analytics.CustomEvent("TimeSwitched", eventData);
        Analytics.FlushEvents();
        Debug.Log("Analytics time switch event submitted");
    }

    private void Switch()
    {
        GlobalData.TimeTenseData.SwitchTimeTense();
        // TODO(keyi): refactor using pub-sub
        OnTimeSwitch();
    }
}
