using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ChangeableItem : MonoBehaviour
{
    [SerializeField]
    private Color presentColor = Color.white;
    [SerializeField]
    private Color pastColor = new (0.0745f, 0.0824f, 0.0980f);

    private SpriteRenderer[] renderers;

    private Subscriber<TimeTense> subscriber;

    // Start is called before the first frame update
    private void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) throw new Exception("This script is not applicable to component without SpriteRenderer");

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);
    }

    private void OnDestroy()
    {
        subscriber?.Unsubscribe(OnTimeSwitch);
    }

    private void OnTimeSwitch(TimeTense tt)
    {
        foreach (var r in renderers)
        {
            r.color = tt == TimeTense.PRESENT ? presentColor : pastColor;
        }
    }
}
