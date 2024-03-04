using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;
using UnityEngine.Analytics;


public class TimeSwitch : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    private static readonly Dictionary<TimeTense, string> ChangeableCalls = new Dictionary<TimeTense, string>()
    {
        { TimeTense.PRESENT, "OnPresent" },
        { TimeTense.PAST, "OnPast" }
    };

    public GameObject presentObjects;
    public GameObject pastObjects;
    public GameObject ChangeableObjects;

    void Start()
    {
        pastObjects.SetActive(true);
        foreach (Transform pastTransform in pastObjects.transform)
        {
            GameObject pastObj = pastTransform.gameObject;
            pastObj.SetActive(false);
        }

        presentObjects.SetActive(true);
        label.text = "Present";
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeSwitch"))
        {
            Switch();
        }
    }

    private void Switch()
    {
        // TODO(keyi): put following logics into `TimeTenseDataManger`
        GlobalData.TimeTenseData.SwitchTimeTense();

        // update object groups
        // pastObjects.SetActive(GlobalData.TimeTenseData.IsPast());
        // presentObjects.SetActive(GlobalData.TimeTenseData.IsPresent());
        foreach (Transform pastTransform in pastObjects.transform)
        {
            GameObject pastObj = pastTransform.gameObject;
            pastObj.SetActive(GlobalData.TimeTenseData.IsPast());
        }

        foreach (Transform presentTransform in presentObjects.transform)
        {
            GameObject presentObj = presentTransform.gameObject;
            presentObj.SetActive(GlobalData.TimeTenseData.IsPresent());

            
        }

        ChangeableObjects.BroadcastMessage(ChangeableCalls[GlobalData.TimeTenseData.GetTimeTense()]);

        // update text and background color 
        label.text = GlobalData.TimeTenseData.GetDisplayText();
        Camera.main.backgroundColor = GlobalData.TimeTenseData.GetBackgroundColor();



        // Analytics
        GlobalData.numberOfTimeSwitches += 1;
        var eventData = new Dictionary<string, object>();
        eventData["LastCheckpointBeforeTimeSwitch"] = GlobalData.LastCheckpointPosition;

        Analytics.CustomEvent("TimeSwitched", eventData);
    }
}
