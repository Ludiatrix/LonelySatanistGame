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

        private bool initialized = false;

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            PhaseEvents.StorePhaseStarted?.AddListener(OnStorePhaseStarted);
            UIEvents.StoreButtonClicked?.AddListener(OnStoreButtonClicked);
            
            CardEvents.BuyCardSuccessResponse?.AddListener(OnBuyCardSuccessResponse);
            CardEvents.BuyCardFailedResponse?.AddListener(OnBuyCardFailedResponse);
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

            // On a successful purchase, close the description box and lower the frame.
            descriptionPanel.Close();
            frameMover.MoveToStart(2.0f);
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
            
            // And space it out enough to reach the next row
            CreateNullStoreItem();

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

            // The card click already toggled the description panel before firing this,
            // so its open state tells us which way to move the frame:
            //   open (a card is selected)   -> slide up to the target
            //   closed (card was deselected) -> slide back down to the start
            if (descriptionPanel != null && descriptionPanel.IsOpen)
                frameMover.MoveToTarget(moverTransformTarget.position, 2.0f);
            else
                frameMover.MoveToStart(2.0f);
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

        private void CreateNullStoreItem()
        {
            var gameObject = new GameObject("spacer");
            gameObject.AddComponent<RectTransform>();
            var storeItem = gameObject.AddComponent<StorePagePopulator>();
            storeItem.transform.parent = storeGridTransform;
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
