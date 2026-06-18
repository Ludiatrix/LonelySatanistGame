using LSG.Core;
using LSG.UI;
using LSG.Utils;
using UnityEngine;

namespace LSG.Phases
{
    /*
     * TODO: Pages are drawn one by one. The player chooses to "Keep Reading" to gain more power/tape milestones or "Stop" to lock in their power meter.
     * If they exceed the safe threshold of the White suit, their summoning attempt calls the dog and the player gets dragged to Hell (bad ending).
     */
    
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
        
        public override void StartPhase()
        {
            Debug.Log("[SummoningPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
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
            Debug.Log("[Summoning Phase] Turning the Page...");
            GameObject page = Instantiate(PagePrefab, PagesTransform, false);
            page.GetComponent<PageFacade>().Inject(PageTurnDestinationTransform);
            Debug.Log("[SummoningPhase] Page Turned!");
            GameEvents.PageRead?.Invoke();
        }

        public void KeepReading()
        {
            TurnPage();
        }

        /// <summary>
        /// Stopping locks in the Power meter and performs the act of Summoning.
        /// TODO: Conduct Summoning!
        /// </summary>
        public void StopReading()
        {
            Debug.Log("[Summoning Phase] The Pages have been read. Now let's see what the chasm of hell brings forth!");
        }
        
    }
}
