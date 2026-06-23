using System;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Gain 2 Sanity
    /// Bane: For the rest of the game, the first 1 power white card played during a summoning round causes -1 Sanity loss.
    /// </summary>
    public class PrioceptEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;
        
        private bool _firstWhiteSuitPlayed = false;

        private void OnEnable()
        {
            CardEvents.CardPlayed?.AddListener(OnCardPlayed);
            PhaseEvents.SummoningPhaseEnded?.AddListener(OnSummoningPhaseEnded);
        }

        private void OnDisable()
        {
            CardEvents.CardPlayed?.RemoveListener(OnCardPlayed);
            PhaseEvents.SummoningPhaseEnded?.RemoveListener(OnSummoningPhaseEnded);
        }

        private void OnSummoningPhaseEnded()
        {
            _firstWhiteSuitPlayed = false;
        }

        private void Start()
        {
            boonPayload = new ModifierPayload
            {
                Sanity = 2
            };
            
            banePayload = new ModifierPayload
            {
                Sanity = -1
            };
        }
        
        private void OnCardPlayed(CardData playedCard)
        {
            if (_firstWhiteSuitPlayed) return;
            
            if (playedCard.Suit == Enums.Suit.White)
            {
                _firstWhiteSuitPlayed = true;
                ApplyBane();
            }
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