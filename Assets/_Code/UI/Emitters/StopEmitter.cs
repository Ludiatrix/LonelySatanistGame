using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to Stop.
    /// </summary>
    public class StopEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[StopEmitter] Firing the Stop Event!");
            GameEvents.StopChosen?.Invoke();
        }
    }
}