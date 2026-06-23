using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Gain 3 Tape
    /// Bane: Unspent Tape is lost at the beginning of each new summoning round
    /// </summary>
    public class SweenglyneEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        private PlayerEconomy _playerEconomy = null;

        private void OnEnable()
        {
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
        }

        private void OnDisable()
        {
            PhaseEvents.SummoningPhaseStarted?.RemoveListener(OnSummoningPhaseStarted);
        }

        private void Start()
        {
            if (_playerEconomy == null)
            {
                _playerEconomy = DataManager.Instance.PlayerEconomySource;
            }
            
            boonPayload = new ModifierPayload
            {
                Tape = 3
            };
            
            //Set boon modifier
            boonPayload.Tape = 3;
            
            ApplyBoon();
        }

        private void OnSummoningPhaseStarted()
        {
            ApplyBane();
        }

        public void ApplyBoon()
        {
            EconomyEvents.SendPayload?.Invoke(boonPayload);
        }

        public void ApplyBane()
        {
            _playerEconomy.Tape = 0;
        }
    }
}