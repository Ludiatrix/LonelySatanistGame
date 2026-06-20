using UnityEngine;

namespace LSG.Phases
{
    public class StorePhase : Phase
    {
        [SerializeField] private GameObject Container;

        public override void StartPhase()
        {
            Debug.Log("[StorePhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
        }

        public override void EndPhase()
        {
            Debug.Log("[StorePhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
        }
    }
}
