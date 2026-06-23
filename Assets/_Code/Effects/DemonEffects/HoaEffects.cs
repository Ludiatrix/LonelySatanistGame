using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
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
        [SerializeField] private ModifierPayload banePayload;

        // Applied each time an orange page is read. Must NOT mutate the page's CardData
        // asset: those are shared ScriptableObjects and edits persist across summonings
        // and even across editor play sessions, permanently corrupting the card's power.
        private readonly ModifierPayload _orangeBoost = new ModifierPayload { Power = 1 };

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
            banePayload = new ModifierPayload();
        }

        private void OnCardTaken(CardData takenCard)
        {
            if (takenCard == null) return;
            if (takenCard.Suit != Enums.Suit.Orange) return;

            // Grant +1 power transiently via a payload rather than editing the asset.
            EconomyEvents.SendPayload?.Invoke(_orangeBoost);
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