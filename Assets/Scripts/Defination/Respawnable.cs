using System;
using UnityEngine;

namespace Game
{
    public abstract class RespawnableMonoBehaviour: MonoBehaviour
    {
        private Subscriber<bool> respawnableSubscriber;

        virtual public void Start()
        {
            respawnableSubscriber = GlobalData.CheckPointData.CreateResetSignalSubscriber();
            respawnableSubscriber.Subscribe(OnRespawn);
        }

        virtual public void OnDestroy()
        {
            respawnableSubscriber?.Unsubscribe(OnRespawn);
        }


        abstract public void OnRespawn(bool r);
    }
}
