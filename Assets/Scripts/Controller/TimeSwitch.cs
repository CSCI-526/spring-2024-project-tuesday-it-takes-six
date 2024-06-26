using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Game;


public class TimeSwitch : RespawnableMonoBehaviour
{
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

    private Subscriber<TimeTense> subscriber;

    private readonly Color GREY = new (0.6f, 0.6f, 0.6f);

    override public void Start()
    {
        player = GameObject.Find("Player");

        base.Start();

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);

        ColorFloors();
    }

    override public void OnDestroy()
    {
        base.OnDestroy();
        subscriber?.Unsubscribe(OnTimeSwitch);
    }

    override public void OnRespawn(bool _)
    {
        if (GlobalData.TimeTenseData.IsPast())
        {
            GlobalData.TimeTenseData.SwitchTimeTense();
        }       
    }

    private void ColorFloors()
    {
        var presentFloors = Utils.FindChildrenWithTag(presentObjects, "Floor");
        var presentFloorsRender = Utils.GetAllComponents<SpriteRenderer>(presentFloors);
        var pastFloors = Utils.FindChildrenWithTag(pastObjects, "Floor");
        var pastFloorsRender = Utils.GetAllComponents<SpriteRenderer>(pastFloors);

        foreach (var r in presentFloorsRender)
        {
            if (r.gameObject.CompareTag("Floor")) r.color = GREY;
        }

        foreach (var r in pastFloorsRender)
        {
            if (r.gameObject.CompareTag("Floor")) r.color = GREY;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("TimeSwitch"))
        {
            GlobalData.TimeTenseData.SwitchTimeTense();
            // reported it by user keystrokes
            // if put in `OnTimeSwitch`, it will record non-user triggered time switch
            ReportAnalytics();
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
    }

    private void ReportAnalytics()
    {
        // Analytics
        GlobalData.numberOfTimeSwitches += 1;
        GlobalData.AnalyticsManager.Send("timeSwitch");
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
