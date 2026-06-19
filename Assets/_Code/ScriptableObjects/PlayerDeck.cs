using System;
using System.Collections.Generic;
using System.Linq;
using LSG.Core;
using UnityEngine;

namespace LSG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerDeck", menuName = "LSG/Make a Deck")]
    public class PlayerDeck : ScriptableObject
    {
        [SerializeField] private List<CardData> _playerDeck = new List<CardData>();
        private readonly List<CardData> _playedCards = new List<CardData>();
        private readonly List<CardData> _cardLibrary = new List<CardData>();

        public List<CardData> PlayedCards => _playedCards;

        private void OnEnable()
        {
            CardEvents.AddRandomCard.AddListener(OnAddRandomCard);
        }

        /*
         * Adds a random card.
         * Can set if we must search for a specific suit
         * and if that card must not already exist in the Player's deck.
         *
         * If you are here and are noticing that this code is unfinished for other cases, then congratulations!
         * You're absolutely correct, however this is a game jam and unless the design calls for it. I ain't makin' it!
         */
        private void OnAddRandomCard(Enums.Suit suit = Enums.Suit.None, bool mustBeUnique = false)
        {
            // first check as it binds us to Player Deck or not
            if (mustBeUnique)
            {
                if (suit != Enums.Suit.None)
                {
                    List<CardData> cardsOfThatSuit = new List<CardData>();
                
                    // search player deck for suits of that color
                    foreach (var thisCard in _playerDeck)
                    {
                        if (thisCard.Suit == suit)
                        {
                            cardsOfThatSuit.Add(thisCard);
                        }
                    }
                    
                    // now with our list we can search the library for the first card that isn't in our deck of the same suit!
                    foreach (var card in _cardLibrary)
                    {
                        if (card.Suit == suit)
                        {
                            if (!cardsOfThatSuit.Contains(card))
                            {
                                // Done!
                                AddCardToPlayerDeck(card);
                                return;
                            }
                        }
                    }
                }
            }
        }
        
        public void AddCardToPlayerDeck(CardData card)
        {
            _playerDeck.Add(card);
            _playerDeck.Shuffle();
            GameEvents.PageAdded?.Invoke(card);
        }

        public CardData TakeCardFromPlayerDeck()
        {
            if (_playerDeck.Count == 0)
            {
                Debug.Log($"[PlayerDeck] There are no more pages... how did you do that? Ah well, you lose!");
                // TODO: Add lose condition here
                return null;
            }
            CardData card = _playerDeck[_playerDeck.Count - 1]; // Riza: fun fact getting the end index is faster than the first
            _playedCards.Add(card);
            _playerDeck.RemoveAt(_playerDeck.Count - 1);
            GameEvents.CardTaken?.Invoke(card);
            return card;
        }
        
        public void Shuffle()
        {
            _playerDeck.Shuffle();
        }

        public void Reset()
        {
            var defaultDeck = Resources.Load<PlayerDeck>($"DefaultDeck");
            _playerDeck = defaultDeck._playerDeck;
            Shuffle();
        }
    }
}
