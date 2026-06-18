using System;
using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to start the game.
    /// </summary>
    public class StartEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[StartEmitter] Firing the StartGame Event! This will take us to the setup phase!");
            GameEvents.StartGame?.Invoke(Enums.GameState.SetupPhase);
        }
    }
}
