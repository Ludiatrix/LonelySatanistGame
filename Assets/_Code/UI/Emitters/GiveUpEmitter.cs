using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to Give Up.
    /// </summary>
    public class GiveUpEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[GiveUpEmitter] Firing the KeepReadingChosen Event!");
            GameEvents.GiveUpChosen?.Invoke();
        }
    }
}