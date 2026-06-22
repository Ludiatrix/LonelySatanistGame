using System;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    public class SetAsideCanvasController : MonoBehaviour
    {
        [SerializeField] private Image imageOne, imageTwo;

        public CardData dataOne, dataTwo;

        void OnEnable()
        {
            GameEvents.PlaceCardsInSetAsideArea?.AddListener(OnPlaceCardsInSetAsideArea);
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
        }

        private void OnPlaceCardsInSetAsideArea(CardData data)
        {
            if (dataOne is null)
            {
                dataOne = data;
                imageOne.enabled = true;
                imageOne.sprite = data.PageImage;
            } else
            {
                dataTwo = data;
                imageTwo.enabled = true;
                imageTwo.sprite = data.PageImage;
            }
        }

        private void OnSummoningPhaseStarted()
        {
            if (dataOne != null)
            {
                DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(dataOne);
            }

            if (dataTwo != null)
            {
                DataManager.Instance.PlayerDeckSource.AddCardToPlayerDeck(dataTwo);
            }

            imageOne.enabled = false;
            imageTwo.enabled = false;
            dataOne = null;
            dataTwo = null;
        }
    }
}
