using System;
using UnityEngine;

namespace Game
{
    public abstract class ResetableMonoBehaviour: MonoBehaviour
    {
        private Subscriber<bool> resetSubSubscriber;

        virtual public void Start()
        {
            resetSubSubscriber = GlobalData.CheckPointData.CreateResetSignalSubscriber();
            resetSubSubscriber.Subscribe(OnReset);
        }

        virtual public void OnDestroy()
        {
            resetSubSubscriber?.Unsubscribe(OnReset);
        }


        abstract public void OnReset(bool r);
    }
}
