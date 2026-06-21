using LSG.Core;
using UnityEngine;

namespace LSG.UI.Emitters
{
    /// <summary>
    /// Signal class used by a button or other trigger used to communicate to GameEvents.cs in order to change Game State.
    /// </summary>
    public class ChangeGameStateEmitter : MonoBehaviour
    {
        [SerializeField] private Enums.GameState gameStateToChangeTo = Enums.GameState.TitlePhase;

        public void Emit()
        {
            GameEvents.ChangeState?.Invoke(gameStateToChangeTo);
        }
    }
}
