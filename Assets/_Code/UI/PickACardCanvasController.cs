using System;
using System.Collections.Generic;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    public class PickACardCanvasController : MonoBehaviour
    {
        public GameObject container;
        public GameObject horizontalContainer;
        public List<GameObject> pickablePages = new List<GameObject>();
        public GameObject pagePrefab;
        public GameObject pickButton;
        public GameObject closeButton;

        // Dirty hack to turn the necronomicon on/off for our purposes
        public GameObject SummoningContainer;

        private PickACardPayload _pickACardPayload = null;

        private CardData _selectedCard = null;
        private int _amountChosen = 0;

        private List<CardData> _chosenCards = new List<CardData>();


        void OnEnable()
        {
            GameEvents.PickACard?.AddListener(OnPickACard);
        }

        private void OnPickACard(PickACardPayload payload)
        {
            container.SetActive(true);
            _pickACardPayload = payload;
            UIEvents.ToggleDialogueWindow?.Invoke(true);
            UIEvents.ToggleResourceUI?.Invoke(false);
            UIEvents.SetNamePlateText?.Invoke("Smol");
            UIEvents.SetDialogueText?.Invoke(payload.Reason);
            UIEvents.DisableButtons?.Invoke();
            SummoningContainer.SetActive(false);

            foreach (var card in payload.Cards)
            {
                GeneratePickablePage(card);
            }
        }

        private void GeneratePickablePage(CardData card)
        {
            var page = Instantiate(pagePrefab, horizontalContainer.transform);

            page.GetComponent<PageFacade>().Inject(card);
            page.GetComponent<Button>().onClick.AddListener(() => LoadCardDescription(card));
            pickablePages.Add(page);
        }

        public void LoadCardDescription(CardData data)
        {
            UIEvents.SetNamePlateText?.Invoke(data.CardWord);
            UIEvents.SetDialogueText?.Invoke(data.CardEffect);
            _selectedCard = data;
            pickButton.SetActive(true);
        }

        public void Pick()
        {
            if (_amountChosen < _pickACardPayload.AmountAbleToChoose)
            {
                _amountChosen++;
                foreach (var pickablePage in pickablePages)
                {
                    if (pickablePage.GetComponent<PageFacade>().cardData == _selectedCard)
                    {
                        Destroy(pickablePage);
                    }
                }
                closeButton.SetActive(true);
                _chosenCards.Add(_selectedCard);
            } else
            {
                Close();
            }
        }

        public void ClearCardDescription()
        {
            UIEvents.SetNamePlateText?.Invoke("Smol");
            UIEvents.SetDialogueText?.Invoke(_pickACardPayload.Reason);
        }
        
        public void Close()
        {
            ResolveAfterEffects(_pickACardPayload.StateAfterChoice);
            UIEvents.ToggleResourceUI?.Invoke(true);
            _pickACardPayload = null;
            _selectedCard = null;
            _amountChosen = 0;

            foreach (var page in pickablePages)
            {
                Destroy(page);
            }
            pickablePages.Clear();
            container.SetActive(false);
            closeButton.SetActive(false);
            UIEvents.ToggleSummoningButtons?.Invoke(true);
            SummoningContainer.SetActive(true);
        }

        private void ResolveAfterEffects(Enums.PickACardAfterEffectState stateAfterChoice)
        {
            switch (stateAfterChoice)
            {
                case Enums.PickACardAfterEffectState.ShuffleRestIntoDeck:
                    ShuffleRest();
                    break;
                case Enums.PickACardAfterEffectState.SetAsideTheRest:
                    SetAsideRest();
                    break;
                case Enums.PickACardAfterEffectState.PutOnBottom:
                    PutOnBottom();
                    break;
                default:
                    Debug.LogWarning($"lmao no");
                    break;
            }
        }

        private void ShuffleRest()
        {
            foreach (var card in _pickACardPayload.Cards)
            {
                if (!_chosenCards.Contains(card))
                {
                    DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(card);
                }
            }

            DataManager.Instance.PlayerDeckSource.playerDeck.Shuffle();
        }

        private void SetAsideRest()
        {
            foreach (var card in _pickACardPayload.Cards)
            {
                if (!_chosenCards.Contains(card))
                {
                    GameEvents.PlaceCardsInSetAsideArea?.Invoke(card);
                }
            }
        }

        private void PutOnBottom()
        {
            foreach (var card in _pickACardPayload.Cards)
            {
                if (!_chosenCards.Contains(card))
                {
                    DataManager.Instance.PlayerDeckSource.playerDeck.Add(card);
                }
            }
        }
    }
}
