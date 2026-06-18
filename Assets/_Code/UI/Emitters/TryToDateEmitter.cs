using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to Try to Date.
    /// </summary>
    public class TryToDateEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[TryToDateEmitter] Firing the KeepReadingChosen Event!");
            GameEvents.TryToDateChosen?.Invoke();
        }
    }
}