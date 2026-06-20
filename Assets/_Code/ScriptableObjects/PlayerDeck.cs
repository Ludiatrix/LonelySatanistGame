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
        // All the cards in the deck
        public List<CardData> CardLibrary = new List<CardData>();
        public List<CardData> ShopList = new List<CardData>();
        public List<CardData> playerDeck = new List<CardData>();
        public List<CardData> playedCards = new List<CardData>();

        public CardList DefaultLibrary;
        public CardList DefaultShopList;
        public CardList DefaultDeck;

        public CardData[] PeekAheadAtLibrary(int peekAheadLength = 1)
        {
            List<CardData> peekedCards = new List<CardData>();
            
            for (int i = 0; i < peekAheadLength; i++)
            {
                peekedCards.Add(CardLibrary[i]);
            }

            return peekedCards.ToArray();
        }
        
        //Add a Card back to the CardLibrary
        private void ReturnCardToLibrary(CardData cardToReturn)
        {
            if (CardLibrary.Contains(cardToReturn))
            {
                Debug.Log($"[PlayerDeck] Tried to return {cardToReturn}, but it already exists!");
            }
            else
            {
                CardLibrary.Add(cardToReturn);
            }
        }

        // Pull a Card from the CardLibrary
        private CardData PullCardFromLibrary(CardData wantedCard = null)
        {
            if (wantedCard == null)
            {
                var cardToPull = CardLibrary[0];
                CardLibrary.RemoveAt(0);
                return cardToPull;
            }

            foreach (var card in CardLibrary)
            {
                if (card == wantedCard)
                {
                    return card;
                }
            }

            return null;
        }
        
        public CardData[] PeekAheadAtPlayerDeck(int peekAheadAmount = 1)
        {
            List<CardData> peekedCards = new List<CardData>();
            
            for (int i = 0; i < peekAheadAmount; i++)
            {
                peekedCards.Add(playerDeck[i]);
            }

            return peekedCards.ToArray();
        }
        

        private void AddCardToPlayerDeck(CardData card)
        {
            playerDeck.Add(card);
            playerDeck.Shuffle();
            GameEvents.PageAdded?.Invoke(card);
        }
        
        public CardData TakeCardFromPlayerDeck()
        {
            if (playerDeck.Count == 0)
            {
                Debug.Log($"[PlayerDeck] There are no more pages... how did you do that? Ah well, you lose!");
                // TODO: Add lose condition here
                return null;
            }
            CardData card = playerDeck[playerDeck.Count - 1]; // Riza: fun fact getting the end index is faster than the first
            playedCards.Add(card);
            playerDeck.RemoveAt(playerDeck.Count - 1);
            GameEvents.CardTaken?.Invoke(card);
            return card;
        }
        
        public void Reset()
        {
            LoadDefaultLibrary();
            LoadDefaultShopList();
            LoadDefaultDeck();
            playedCards.Clear();
        }
        
        private void LoadDefaultLibrary()
        {
            CardLibrary.Clear();
            foreach (var card in DefaultLibrary.Cards)
            {
                CardLibrary.Add(card);
            }
        }
        
        private void LoadDefaultShopList()
        {
            ShopList.Clear();
            foreach (var card in DefaultShopList.Cards)
            {
                ShopList.Add(card);
            }
        }

        private void LoadDefaultDeck()
        {
            playerDeck.Clear();
            foreach (var card in DefaultDeck.Cards)
            {
                AddCardToPlayerDeck(PullCardFromLibrary(card));
            }
            
            Shuffle();
        }
        
        public void Shuffle()
        {
            playerDeck.Shuffle();
        }

        public int PlayerDeckCount => playerDeck.Count;

        private void OnEnable()
        {
            CardEvents.AddRandomCard.AddListener(OnAddRandomCard);
            CardEvents.RemoveRandomCard.AddListener(OnRemoveRandomCard);
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
                    foreach (var thisCard in playerDeck)
                    {
                        if (thisCard.Suit == suit)
                        {
                            cardsOfThatSuit.Add(thisCard);
                        }
                    }
                    
                    // now with our list we can search the CardLibrary for the first card that isn't in our deck of the same suit!
                    foreach (var card in CardLibrary)
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

        private void OnRemoveRandomCard(Enums.Suit suitToIgnore, bool canBeBoughtAgain = false)
        {
            
        }
        
        

        
    }
}
