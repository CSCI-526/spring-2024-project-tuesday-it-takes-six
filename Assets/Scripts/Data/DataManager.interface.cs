using System;
using UnityEngine.PlayerLoop;

namespace Game
{
    interface IDataManager
    {
        /// <summary>
        /// will be called when game is just started/reloaded
        /// </summary>
        void Init();
    }
}