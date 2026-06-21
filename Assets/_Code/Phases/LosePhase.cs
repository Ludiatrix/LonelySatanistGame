using LSG.Core;
using UnityEngine;

namespace LSG.Phases
{
    public class LosePhase : Phase
    {
        [SerializeField] private GameObject container;

        public override void StartPhase()
        {
            base.StartPhase();
            container.SetActive(true);
            UIEvents.ToggleDialogueWindow?.Invoke(false);
        }

        public override void EndPhase()
        {
            base.EndPhase();
            container.SetActive(false);
        }
    }
}
