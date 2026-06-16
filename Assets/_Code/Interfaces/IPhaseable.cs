using UnityEngine;

namespace LSG.Interfaces
{
    /// <summary>
    /// Interface for Phases. Holds the contract for starting and ending phases.
    /// </summary>
    public interface IPhaseable
    {
        void StartPhase();
        void EndPhase();
    }
}