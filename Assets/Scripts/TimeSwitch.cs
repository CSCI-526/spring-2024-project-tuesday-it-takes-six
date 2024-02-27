using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;


public class TimeSwitch : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    private Dictionary<string, string> changableCalls;// = new Dictionary<TimeTense>

    public GameObject presentObjects;
    public GameObject pastObjects;
    public GameObject changableObjects;

    // please not use var starting with an underscore inside/outside of class
    private TimeTense _tt;
    private TimeTense tt
    {
        get { return _tt; }
        set
        {
            _tt = value;
            pastObjects.SetActive(value == TimeTense.PAST);
            presentObjects.SetActive(value == TimeTense.PRESENT);

            label.text = value == TimeTense.PRESENT ? "Present" : "Past";
        }
    }


    void Start()
    {
        tt = TimeTense.PRESENT;

        changableCalls = new Dictionary<string, string>();
        changableCalls.Add("Present", "OnPresent");
        changableCalls.Add("Past", "OnPast");
        GlobalData.Instance.tt = TimeTense.PRESENT;
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
        tt = tt == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;
        // change global data to let other objects know current time
        GlobalData.Instance.tt = GlobalData.Instance.tt == TimeTense.PRESENT ? TimeTense.PAST : TimeTense.PRESENT;

        changableObjects.BroadcastMessage(changableCalls[label.text]);
    }
}
