using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to open the Necronomicon.
    /// </summary>
    public class StartReadingEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[StartReadingEmitter] Firing the StartReading Event! This will OPEN THE NECRONOMICON!");
            GameEvents.StartReading?.Invoke();
        }
    }
}
