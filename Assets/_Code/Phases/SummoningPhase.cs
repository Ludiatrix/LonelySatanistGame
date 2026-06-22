using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// The Summoning Phase is governed by two primary actions: "Keep Reading" and "Stop".
    /// </summary>
    public class SummoningPhase : Phase
    {
        public GameObject Container;

        private void OnEnable()
        {
            GameEvents.KeepReadingChosen?.AddListener(OnKeepReadingChosen);
            GameEvents.StopChosen?.AddListener(OnStopChosen);
        }

        private void OnDisable()
        {
            GameEvents.KeepReadingChosen?.RemoveListener(OnKeepReadingChosen);
            GameEvents.StopChosen?.RemoveListener(OnStopChosen);
        }

        public override void StartPhase()
        {
            Debug.Log("[SummoningPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            DataManager.Instance.PlayerEconomySource.Power = 0;
            if (DataManager.Instance.PlayerDeckSource.playedCards.Count > 0)
            {
                DataManager.Instance.PlayerDeckSource.ReshufflePlayedCardsToPlayerDeck();
            }
            PhaseEvents.SummoningPhaseStarted?.Invoke();
            UIEvents.ToggleResourceUI?.Invoke(true);
            GeneratePage();
        }

        private void GeneratePage()
        {
            CardData data = DataManager.Instance.PlayerDeckSource.TakeCardFromPlayerDeck();
            GenerateSpecificPage(data);
        }

        private void GenerateSpecificPage(CardData data)
        {
            
            Debug.Log($"[Summoning Phase] We have page: {data.name} with word {data.CardWord} and suit {data.Suit.ToString()}");

            UIEvents.DisplayNecronomiconPage?.Invoke(data);
            SetCardTextOnDialogueWindow(data);
            ApplyCardEffects(data);

            GameEvents.PageRead?.Invoke();
        }

        private void SetCardTextOnDialogueWindow(CardData data)
        {
            UIEvents.SetNamePlateText?.Invoke(data.CardWord);
            UIEvents.SetDialogueText?.Invoke(data.CardEffect);
        }

        private void ApplyCardEffects(CardData data)
        {
            EconomyEvents.SendPayload?.Invoke(data.PageModifier);
            DataManager.Instance.EffectDataSource.ResolveCardEffect(data);
        }

        private void TurnPage()
        {
            UIEvents.TurnNecronomiconPage?.Invoke();
            GeneratePage();
        }

        public override void EndPhase()
        {
            Debug.Log("[SummoningPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
            PhaseEvents.SummoningPhaseEnded?.Invoke();
            UIEvents.ToggleNecronomicon?.Invoke(false);
        }

        private void OnKeepReadingChosen()
        {
            TurnPage();
        }
        
        private void OnStopChosen()
        {
            Debug.Log("[Summoning Phase] The Pages have been read. Now let's see what the chasm of hell brings forth!");
            UIEvents.DisableButtons?.Invoke();
            UIEvents.ToggleDialogueWindow?.Invoke(false);
            GameEvents.ChangeState?.Invoke(Enums.GameState.EncounterPhase);
        }
    }
}
