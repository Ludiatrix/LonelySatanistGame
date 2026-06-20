using System;
using System.Collections.Generic;
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

        private void Start()
        {
        }

        private void OnStorePhaseStarted()
        {
            GenerateCardsToBuy();
            GenerateBoughtCards();
        }

        private void OnStorePhaseEnded()
        {
            ClearStoreCards();
        }

        private void GenerateCardsToBuy() => GenerateStoreItems(DataManager.Instance.PlayerDeckSource.CardLibrary, false);

        private void GenerateBoughtCards()
        {
            GenerateStoreItems(DataManager.Instance.PlayerDeckSource.playerDeck, true);
            GenerateStoreItems(DataManager.Instance.PlayerDeckSource.playedCards, true);
        }

        private void GenerateStoreItems(List<CardData> cardsToGenerate, bool owned)
        {
            foreach (var card in cardsToGenerate)
            {
                var storeItem = Instantiate(templateStorePageItem, storeGridTransform);
                storeItem.gameObject.SetActive(true);
                storeItem.SetPageData(card, owned);
            }
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
