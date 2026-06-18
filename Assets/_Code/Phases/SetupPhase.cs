using System;
using LSG.Core;
using LSG.Utils;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// Hook for the SetupPhase interface and holds the container of SetupPhase objects for activation.
    /// </summary>
    public class SetupPhase : Phase
    {
        public GameObject Container;
        public GameObject Necronomicon;
        public GameObject StartReadingButton;

        private Animator _necronomiconAnimator = null;
        private static readonly int SlideUp = Animator.StringToHash("SlideUp");

        private void Awake()
        {
            _necronomiconAnimator = Necronomicon.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameEvents.NecronomiconFinishedSliding?.AddListener(ActivateStartReadingButton);
        }

        private void OnDisable()
        {
            GameEvents.NecronomiconFinishedSliding?.RemoveListener(ActivateStartReadingButton);
        }

        public override void StartPhase()
        {
            Debug.Log("[SetupPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            GameEvents.NecronomiconStartSliding?.Invoke();
            PhaseEvents.SetupPhaseStarted?.Invoke();
        }

        public override void EndPhase()
        {
            Debug.Log("[SetupPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
            PhaseEvents.SetupPhaseEnded?.Invoke();
        }

        private void ActivateStartReadingButton()
        {
            StartReadingButton.SetActive(true);
        }
    }
}
