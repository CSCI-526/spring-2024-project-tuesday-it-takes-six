using System.Collections;
using System.Collections.Generic;


namespace Game
{
    /// <summary>
    /// Every changeable objects must implement this interface
    /// </summary>
    interface IChangeable
    {
        void OnPast();
        void OnPresent();
    }
}
