using System;
using Game;
using UnityEngine;

public class ChangeableItem : MonoBehaviour
{
    [SerializeField]
    private Color presentColor = Color.white;
    [SerializeField]
    private Color pastColor = new (0.0745f, 0.0824f, 0.0980f);

    private SpriteRenderer sr;

    private Subscriber<TimeTense> subscriber;

    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            throw new Exception("This script is not applicable to component without SpriteRenderer");
        }

        subscriber = GlobalData.TimeTenseData.CreateTimeTenseSubscriber();
        subscriber.Subscribe(OnTimeSwitch, true);
    }

    private void OnDestroy()
    {
        subscriber?.Unsubscribe(OnTimeSwitch);
    }

    private void OnTimeSwitch(TimeTense tt)
    {
        sr.color = tt == TimeTense.PRESENT
            ? presentColor
            : pastColor;
    }
}
