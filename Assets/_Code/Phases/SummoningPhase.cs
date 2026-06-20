using LSG.Core;
using LSG.ScriptableObjects;
using LSG.UI;
using UnityEngine;

namespace LSG.Phases
{
    /// <summary>
    /// The Summoning Phase is governed by two primary actions: "Keep Reading" and "Stop".
    /// </summary>
    public class SummoningPhase : Phase
    {
        public GameObject Container;
        
        // Page Generation
        public Transform PagesTransform;
        public GameObject PagePrefab;
        public Transform PageTurnDestinationTransform;

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
            TurnPage(); // Turns the first page
            PhaseEvents.SummoningPhaseStarted?.Invoke();
        }

        public override void EndPhase()
        {
            Debug.Log("[SummoningPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
            PhaseEvents.SummoningPhaseEnded?.Invoke();
        }

        private void TurnPage()
        {
            CardData data = DataManager.Instance.PlayerDeckSource.TakeCardFromPlayerDeck();
            Debug.Log("[Summoning Phase] Turning the Page...");
            GameObject page = Instantiate(PagePrefab, PagesTransform, false);
            Debug.Log($"[Summoning Phase] We have page: {data.name} with word {data.CardWord} and suit {data.Suit.ToString()}");
            page.GetComponent<PageFacade>().Inject(data, PageTurnDestinationTransform);
            Debug.Log("[SummoningPhase] Page Turned!");
            GameEvents.PageRead?.Invoke();
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
