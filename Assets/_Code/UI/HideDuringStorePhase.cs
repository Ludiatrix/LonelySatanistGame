using LSG.Core;
using UnityEngine;

namespace LSG
{
    public class HideDuringStorePhase : MonoBehaviour
    {
        public GameObject ObjectToHide;
        private bool priorEnabledState = true;

        public void OnEnable()
        {
            PhaseEvents.StorePhaseStarted?.AddListener(OnPhaseStarted);
            PhaseEvents.StorePhaseEnded?.AddListener(OnPhaseEnded);
        }

        private void OnPhaseStarted()
        {
            priorEnabledState = ObjectToHide.activeSelf;
            ObjectToHide.SetActive(false);
        }
        private void OnPhaseEnded()
        {
            ObjectToHide.SetActive(priorEnabledState);
        }
    }
}
