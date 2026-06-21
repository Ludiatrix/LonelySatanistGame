using UnityEngine;

namespace LSG.Phases
{
    public class WinPhase : Phase
    {
        [SerializeField] private GameObject container;

        public override void StartPhase()
        {
            base.StartPhase();
            container.SetActive(true);
        }

        public override void EndPhase()
        {
            base.EndPhase();
            container.SetActive(false);
        }
    }
}
