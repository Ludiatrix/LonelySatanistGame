using LSG.Interfaces;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// Base class for phases.
    /// </summary>
    public abstract class Phase : MonoBehaviour, IPhaseable
    {
        public virtual void StartPhase()
        {
        }

        public virtual void EndPhase()
        {
        }
    }
}