using LSG.Interfaces;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// Base class for phases.
    /// </summary>
    public abstract class Phase : MonoBehaviour, IPhaseable
    {
        // Riza: We are running these async on main as we are turning on/off a bunch of things
        public virtual async Awaitable<bool> StartPhase()
        {
            await Awaitable.NextFrameAsync();
            return true;
        }

        public virtual async Awaitable<bool> EndPhase()
        {
            await Awaitable.NextFrameAsync();
            return true;
        }
    }
}