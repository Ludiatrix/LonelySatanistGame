using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Gain 1 Rizz
    /// Bane: The next summoning, the suit images on the white cards and their power counter are invisible.
    /// </summary>
    public class K_visEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;

        private bool _alreadyAppliedEffect = false;

        private void OnEnable()
        {
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
        }

        private void OnSummoningPhaseStarted()
        {
            if (_alreadyAppliedEffect) return;
            
            ApplyBane();
        }

        private void Start()
        {
            boonPayload = new ModifierPayload
            {
                Rizz = 1
            };
        }

        public void ApplyBoon()
        {
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            _alreadyAppliedEffect = true;
            
        }
    }
}