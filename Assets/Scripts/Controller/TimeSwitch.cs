using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;


public class TimeSwitch : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    private static readonly Dictionary<TimeTense, string> changableCalls = new Dictionary<TimeTense, string>()
    {
        { TimeTense.PRESENT, "OnPresent" },
        { TimeTense.PAST, "OnPast" }
    };

    public GameObject presentObjects;
    public GameObject pastObjects;
    public GameObject changableObjects;

    void Start()
    {
        pastObjects.SetActive(false);
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
        pastObjects.SetActive(GlobalData.TimeTenseData.IsPast());
        presentObjects.SetActive(GlobalData.TimeTenseData.IsPresent());

        changableObjects.BroadcastMessage(changableCalls[GlobalData.TimeTenseData.GetTimeTense()]);

        // update text
        label.text = GlobalData.TimeTenseData.GetDisplayText();
    }
}
