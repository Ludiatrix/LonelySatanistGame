using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to Exit Store.
    ///
    /// </summary>
    public class ExitStoreEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[ExitStoreEmitter] Firing the ChangeState Event!");
            GameEvents.ChangeState?.Invoke(Enums.GameState.SummoningPhase);
        }
    }
}