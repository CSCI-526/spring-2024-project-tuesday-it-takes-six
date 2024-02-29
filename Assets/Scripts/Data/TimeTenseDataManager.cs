using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class TimeTenseDataManager
{
    private TimeTense timeTense = TimeTense.PRESENT;

    public bool IsPresent()
    {
        return timeTense == TimeTense.PRESENT;
    }

    public bool IsPast()
    {
        return timeTense == TimeTense.PAST;
    }

    public void SwitchTimeTense()
    {
        timeTense = IsPresent() ? TimeTense.PAST : TimeTense.PRESENT;
    }

    public TimeTense GetTimeTense()
    {
        return timeTense;
    }

    public string GetDisplayText()
    {
        return IsPresent() ? "Present" : "Past";
    }
}
