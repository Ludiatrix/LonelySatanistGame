using LSG.Phases;
using UnityEngine;

namespace LSG.Phases
{
    public class SetupPhase : Phase
    {
        public override void StartPhase()
        {
            Debug.Log("[SetupPhase] Starting Phase!");
            base.StartPhase();
        }

        public override void EndPhase()
        {
            Debug.Log("[SetupPhase] Ending Phase!");
            base.EndPhase();
        }
    }
}
