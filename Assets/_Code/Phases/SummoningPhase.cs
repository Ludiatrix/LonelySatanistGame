using System;
using LSG.Core;
using LSG.ScriptableObjects;
using LSG.UI;
using LSG.Utils;
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
        public PlayerEconomy PlayerEconomy;
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
            GameEvents.ToggleDialogueWindow?.Invoke(true);
            GameEvents.ToggleSummoningButtons?.Invoke(true);
            TurnPage(); // Turns the first page
        }

        public override void EndPhase()
        {
            Debug.Log("[SummoningPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
        }

        private void TurnPage()
        {
            PageData data = PlayerEconomy.PlayerDeckSource.TakePage();
            Debug.Log("[Summoning Phase] Turning the Page...");
            GameObject page = Instantiate(PagePrefab, PagesTransform, false);
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
            GameEvents.DisableButtons?.Invoke();
            GameEvents.ToggleDialogueWindow?.Invoke(false);
            GameEvents.ChangeState?.Invoke(Enums.GameState.EncounterPhase);
        }
        
    }
}
