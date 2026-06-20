using System;
using System.Collections.Generic;
using System.Linq;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.UI
{
    public class StoreController : MonoBehaviour
    {
        public StorePagePopulator templateStorePageItem;
        public Transform storeGridTransform;
        public StorePagePopulator previewPage;
        public CardDescriptionPanel descriptionPanel;

        public List<StorePagePopulator> populatedStorePageItems = new List<StorePagePopulator>();

        private void OnEnable()
        {
            PhaseEvents.StorePhaseStarted?.AddListener(OnStorePhaseStarted);
            PhaseEvents.StorePhaseEnded?.AddListener(OnStorePhaseEnded);
        }

        private void OnStorePhaseStarted()
        {
            // We want to first display the default player deck
            GenerateStoreItems(DataManager.Instance.PlayerDeckSource.DefaultDeck.Cards, true);
            
            // Now we want to get our default shop list
            var shopArr = (CardData[])DataManager.Instance.PlayerDeckSource.DefaultShopList.Cards.Clone();

            List<CardData> ownedCards = new List<CardData>();
            ownedCards.AddRange(DataManager.Instance.PlayerDeckSource.playerDeck);
            ownedCards.AddRange(DataManager.Instance.PlayerDeckSource.playedCards);

            ownedCards = ownedCards.OrderBy(n => n.Suit).OrderBy(n => n.TapeCost).ToList();
            
            foreach (var shopCard in shopArr)
            {
                GenerateStoreItem(shopCard, ownedCards.Contains(shopCard));
            }
        }

        private void OnStorePhaseEnded()
        {
            ClearStoreCards();
        }

        private void GenerateStoreItems(CardData[] cardsToGenerate, bool owned)
        {
            foreach (var card in cardsToGenerate)
            {
                GenerateStoreItem(card, owned);
            }
        }
        
        private void GenerateStoreItem(CardData cardToGenerate, bool owned)
        {
            var storeItem = Instantiate(templateStorePageItem, storeGridTransform);
            storeItem.gameObject.SetActive(true);
            storeItem.SetPageData(cardToGenerate, owned);
        }

        private void ClearStoreCards()
        {
            foreach (var populatedStorePageItem in populatedStorePageItems)
            {
                Destroy(populatedStorePageItem.gameObject);
            }

            populatedStorePageItems.Clear();
        }
    }
}
