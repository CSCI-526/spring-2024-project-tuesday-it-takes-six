using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class TimeTenseDataManager : IDataManager
{
    private TimeTense timeTense = TimeTense.PRESENT;

    private readonly Color PRESENT_BACKGROUND = new (0.2235f, 0.2471f, 0.2941f);
    private readonly Color PAST_BACKGROUND = new (0.0745f, 0.0824f, 0.0980f);

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
        return IsPresent() ? "Time: Present" : "Time: Past";
    }

    /// <summary>
    /// Set time tense to present
    /// </summary>
    public void Init()
    {
        timeTense = TimeTense.PRESENT;
    }

    public Color GetBackgroundColor()
    {
        if (IsPresent())
        {
            return PRESENT_BACKGROUND;
        } else
        {
            return PAST_BACKGROUND;
        }
    }
}
