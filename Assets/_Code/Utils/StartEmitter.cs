using System;
using UnityEngine;

namespace LSG.Utils
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to start the game.
    /// </summary>
    public class StartEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[StartEmitter] Firing the StartGame Event!");
            GameEvents.StartGame?.Invoke();
        }
    }
}
