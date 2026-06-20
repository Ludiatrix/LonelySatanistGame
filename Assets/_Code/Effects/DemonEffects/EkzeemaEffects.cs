using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: Gain 1 Rizz the first time you buy a card (repair a page) each round
    /// Bane: For the rest of the game, Lose 1 Sanity each time you play a blue card
    /// </summary>
    public class EkzeemaEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private bool _cardBought = false;
        
        private void OnEnable()
        {
            CardEvents.CardPlayed?.AddListener(OnCardPlayed);
            CardEvents.CardRepaired?.AddListener(OnCardRepaired);
            PhaseEvents.StorePhaseEnded?.AddListener(OnStorePhaseEnded);
        }

        private void Start()
        {
            // Satisfy the modifiers in the boon and bane
            boonPayload.Rizz = 1;

            banePayload.Sanity = -1;
        }

        private void OnCardRepaired(CardData repairedCard)
        {
            if (_cardBought) return;
            
            ApplyBoon();
            _cardBought = true;
        }
        
        private void OnStorePhaseEnded()
        {
            _cardBought = false;
        }
        
        private void OnCardPlayed(CardData playedCard)
        {
            if (playedCard.Suit == Enums.Suit.Blue)
            {
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