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
            // Change background color
            Color newColor = new Color();
            ColorUtility.TryParseHtmlString("#314D79", out newColor);
            return newColor;
        } else
        {
            // Change background color
            Color newColor = new Color();
            ColorUtility.TryParseHtmlString("#327936", out newColor);
            return newColor;
        }
    }
}
