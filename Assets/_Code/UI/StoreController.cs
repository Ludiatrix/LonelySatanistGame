using System;
using System.Collections.Generic;
using System.Linq;
using LSG.Core;
using LSG.ScriptableObjects;
using LSG.Utils;
using UnityEngine;

namespace LSG.UI
{
    public class StoreController : MonoBehaviour
    {
        public StorePagePopulator templateStorePageItem;
        public Transform storeGridTransform;
        public SmoothMover frameMover;
        public Transform moverTransformTarget;
        public StorePagePopulator previewPage;
        public CardDescriptionPanel descriptionPanel;

        public List<StorePagePopulator> populatedStorePageItems = new List<StorePagePopulator>();

        private bool _buyablePageClicked = false;

        private void OnEnable()
        {
            PhaseEvents.StorePhaseStarted?.AddListener(OnStorePhaseStarted);
            UIEvents.StoreButtonClicked?.AddListener(OnStoreButtonClicked);
            
            CardEvents.BuyCardSuccessResponse?.AddListener(OnBuyCardSuccessResponse);
            CardEvents.BuyCardSuccessResponse?.AddListener(OnBuyCardFailedResponse);
        }

        private void OnBuyCardSuccessResponse(CardData successfulCardData)
        {
            foreach (var populatedStorePageItem in populatedStorePageItems)
            {
                if (populatedStorePageItem.cardData == successfulCardData)
                {
                    populatedStorePageItem.SetPageData(successfulCardData, true);
                    populatedStorePageItem.SuccessPulse();
                }
            }
        }
        
        private void OnBuyCardFailedResponse(CardData failedCardData)
        {
            foreach (var populatedStorePageItem in populatedStorePageItems)
            {
                if (populatedStorePageItem.cardData == failedCardData)
                {
                    populatedStorePageItem.FailPulse();
                }
            }
        }

        private void OnStorePhaseStarted()
        {
            // Make sure the store is reset first
            ClearStoreCards();
            frameMover.ResetPosition();
            descriptionPanel.Close();
            descriptionPanel.DelayNextOpen();
            
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
        
        private void OnStoreButtonClicked()
        {
            if (_buyablePageClicked) return;
            frameMover.MoveToTarget(moverTransformTarget.position, 2.0f);
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
            populatedStorePageItems.Add(storeItem);
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
