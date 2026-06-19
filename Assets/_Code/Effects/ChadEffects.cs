using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Next summoning round begins at +3 Power
    /// Bane: Lose 1 Sanity
    /// </summary>
    public class ChadEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private bool _boonEffectAlreadyApplied = false;

        private void OnEnable()
        {
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
        }

        private void Start()
        {
            boonPayload.Power = 3;
            banePayload.Sanity = -1;
        }

        private void OnSummoningPhaseStarted()
        {
            if (_boonEffectAlreadyApplied) return;
            ApplyBoon();
        }

        public void ApplyBoon()
        {
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            EconomyEvents.SendPayload?.Invoke(banePayload);
        }
    }
}