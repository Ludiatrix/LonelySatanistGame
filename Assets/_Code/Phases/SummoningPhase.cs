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

        // Set when a page read redirects us away from summoning (e.g. a forced
        // Papiyawn / The Book encounter, or running out of pages). Used to abort the
        // rest of the current page's processing so summoning UI doesn't bleed into
        // the encounter.
        private bool _leavingPhase;

        private void OnEnable()
        {
            GameEvents.KeepReadingChosen?.AddListener(OnKeepReadingChosen);
            GameEvents.StopChosen?.AddListener(OnStopChosen);
            GameEvents.ChangeState?.AddListener(OnChangeStateRequested);
        }

        private void OnDisable()
        {
            GameEvents.KeepReadingChosen?.RemoveListener(OnKeepReadingChosen);
            GameEvents.StopChosen?.RemoveListener(OnStopChosen);
            GameEvents.ChangeState?.RemoveListener(OnChangeStateRequested);
        }

        private void OnChangeStateRequested(Enums.GameState target)
        {
            if (target != Enums.GameState.SummoningPhase)
            {
                _leavingPhase = true;
            }
        }

        public override void StartPhase()
        {
            Debug.Log("[SummoningPhase] Starting Phase!");
            base.StartPhase();
            _leavingPhase = false;
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

            // Taking the card may have triggered a forced encounter (Papiyawn) or an
            // empty deck (LosePhase). If so, don't display/process the page.
            if (_leavingPhase || data == null)
            {
                return;
            }

            GenerateSpecificPage(data);
        }

        private void GenerateSpecificPage(CardData data)
        {
            
            Debug.Log($"[Summoning Phase] We have page: {data.name} with word {data.CardWord} and suit {data.Suit.ToString()}");

            UIEvents.DisplayNecronomiconPage?.Invoke(data);
            SetCardTextOnDialogueWindow(data);
            ApplyCardEffects(data);

            // Applying the page's effects may have drained Sanity to 0 and summoned
            // The Book. If we're leaving, don't fire PageRead (rewards/UI for a page
            // we're abandoning).
            if (_leavingPhase)
            {
                return;
            }

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

            // SendPayload can drop Sanity to 0 and summon The Book; if so, stop here
            // rather than resolving the rest of this page's effect.
            if (_leavingPhase)
            {
                return;
            }

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
