using System;
using LSG.Core;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// Hook for the StartPhase interface and holds the container of TitlePhase objects for activation.
    /// </summary>
    public class TitlePhase : Phase
    {
        public GameObject Container;

        public override void StartPhase()
        {
            Debug.Log("[TitlePhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            PhaseEvents.TitlePhaseStarted?.Invoke();
        }

        public override void EndPhase()
        {
            Debug.Log("[TitlePhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
            PhaseEvents.TitlePhaseEnded?.Invoke();
        }
    }
}
