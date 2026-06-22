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
    /// Boon: The next time you get 8 white points, the last white card is shuffled back
	/// into the deck
    /// Bane: Lose 1 Tape (if the player has a tape banked)
    /// </summary>
    public class AdmiralFortranEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private bool _hasRedo = true;

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
            
            banePayload = new ModifierPayload
            {
                Tape = -1
            };
        }

        private void OnCardTaken(CardData takenCard)
        {
            if (!_hasRedo) return;
            if (takenCard.Suit != Enums.Suit.White) return;

            var economy = DataManager.Instance.PlayerEconomySource;
            if (economy.WhiteSuitPoints < 7) return;

            _hasRedo = false;
	
            if (DataManager.Instance.PlayerDeckSource.playedCards.Contains(takenCard))
            {
                DataManager.Instance.PlayerDeckSource.playedCards.Remove(takenCard);
            }

            DataManager.Instance.PlayerDeckSource.playerDeck.Add(takenCard);
            DataManager.Instance.PlayerDeckSource.Shuffle();

            if (economy.WhiteSuitPoints > 0)
            {
                economy.WhiteSuitPoints--;
            }	
            Debug.Log("[AdmiralFortranEffects] Redo triggered: white card returned to deck.");
        }

        public void ApplyBoon()
		{
			// handled in OnCardTaken
		}

        public void ApplyBane()
        {
            EconomyEvents.SendPayload?.Invoke(banePayload);
        }
    }
}