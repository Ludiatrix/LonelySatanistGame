using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to Go to Store.
    ///
    /// TODO: Right now this loops back to Summoning as the store isn't hooked up yet. Hook this up to store when we can :3
    /// </summary>
    public class GoToStoreEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[GoToStoreEmitter] Firing the ChangeState Event!");
            GameEvents.ChangeState?.Invoke(Enums.GameState.SummoningPhase);
        }
    }
}
