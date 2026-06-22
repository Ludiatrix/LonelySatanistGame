using LSG.Classes;
using LSG.Core;
using LSG.Interfaces;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.Effects
{
    /// <summary>
    /// Please update if changed
    /// Boon: For the rest of the game, you will see a warning (either in the dialog box or an icon in the HUD) if the next card to be drawn is white suit.
    /// Bane: For the rest of the game, the first Orange card played each round causes an additional -1 Sanity loss
    /// </summary>
    public class InariEffects : MonoBehaviour, IEffectable
    {
        [SerializeField] private ModifierPayload boonPayload;
        [SerializeField] private ModifierPayload banePayload;

        private bool _warningEnabled;
        private bool _baneApplied;
        private string _lastCardEffectText = string.Empty;

        private void OnEnable()
        {
            GameEvents.CardTaken?.AddListener(OnCardTaken);
            GameEvents.PageRead?.AddListener(OnPageRead);
            PhaseEvents.SummoningPhaseEnded?.AddListener(ResetRoundState);
            _warningEnabled = true;
        }

        private void OnDisable()
        {
            GameEvents.CardTaken?.RemoveListener(OnCardTaken);
            GameEvents.PageRead?.RemoveListener(OnPageRead);
            PhaseEvents.SummoningPhaseEnded?.RemoveListener(ResetRoundState);
        }

        private void Start()
        {
            boonPayload = new ModifierPayload();
            banePayload = new ModifierPayload
            {
                Sanity = -1
            };
        }

        private void OnCardTaken(CardData takenCard)
        {
            if (_baneApplied) return;
            if (takenCard == null) return;

            _lastCardEffectText = takenCard.CardEffect;

            if (takenCard.Suit != Enums.Suit.Orange) return;

            ApplyBane();
        }

        private void OnPageRead()
        {
            if (!_warningEnabled) return;
            if (DataManager.Instance.PlayerDeckSource.PlayerDeckCount == 0) return;

            int deckCount = DataManager.Instance.PlayerDeckSource.PlayerDeckCount;
            CardData nextCard = DataManager.Instance.PlayerDeckSource.playerDeck[deckCount - 1];
            if (nextCard.Suit != Enums.Suit.White) return;

            UIEvents.AppendDialogueText?.Invoke($"The next card to be drawn a dagger suit.");
        }

        private void ResetRoundState()
        {
            _baneApplied = false;
        }

        public void ApplyBoon()
        {
            // handled by event listener
        }

        public void ApplyBane()
        {
            EconomyEvents.SendPayload?.Invoke(banePayload);
			_baneApplied = true;
        }
    }
}