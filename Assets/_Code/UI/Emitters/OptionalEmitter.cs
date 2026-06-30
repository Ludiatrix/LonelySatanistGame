using LSG.Core;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Signal class used by the "Use Optional Power" button to tell GameEvents.cs that the
    /// player wants to use the current page's optional effect.
    /// </summary>
    public class OptionalEmitter : MonoBehaviour
    {
        public void Emit()
        {
            Debug.Log("[OptionalEmitter] Firing the OptionalChosen Event!");
            GameEvents.OptionalChosen?.Invoke();
        }
    }
}
