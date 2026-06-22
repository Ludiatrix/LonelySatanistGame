using System.Collections.Generic;
using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: All [orange suit] pages are +1 power for the rest of this game.
    /// Bane: One of your non-white pages is randomly removed and cannot be purchased again this game.
    /// </summary>
    public class HoaEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;
        private readonly HashSet<CardData> _boostedOrangeCards = new HashSet<CardData>();

        private void OnEnable()
        {
            GameEvents.CardTaken?.AddListener(OnCardTaken);
        }

        private void OnDisable()
        {
            GameEvents.CardTaken?.RemoveListener(OnCardTaken);
        }

        private void Start()
        {
            boonPayload = new ModifierPayload();
            banePayload = new ModifierPayload();
        }

        private void OnCardTaken(CardData takenCard)
        {
            if (takenCard == null) return;
            if (takenCard.Suit != Enums.Suit.Orange) return;
            if (_boostedOrangeCards.Contains(takenCard)) return;

            takenCard.PageModifier.Power += 1;
            _boostedOrangeCards.Add(takenCard);
        }

        public void ApplyBoon()
        {
            // card event listener
        }

        public void ApplyBane()
        {
            CardEvents.RemoveRandomCard?.Invoke(Enums.Suit.White, false);
        }
    }
}