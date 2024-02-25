using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;


public class TimeSwitch : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    public GameObject presentObjects;
    public GameObject pastObjects;

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
        tt = tt == TimeTense.PRESENT
            ? TimeTense.PAST
            : TimeTense.PRESENT;
    }
}
