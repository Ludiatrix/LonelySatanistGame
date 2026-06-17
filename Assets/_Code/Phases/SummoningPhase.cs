using LSG.Core;
using LSG.ScriptableObjects;
using LSG.Utils;
using UnityEngine;

namespace LSG.Phases
{
    /*
     * TODO: Pages are drawn one by one. The player chooses to "Keep Reading" to gain more power/tape milestones or "Stop" to lock in their power meter.
     * If they exceed the safe threshold of the White suit, their summoning attempt calls the dog and the player gets dragged to Hell (bad ending).
     */
    
    /// <summary>
    /// The Summoning Phase is governed by two primary actions: "Keep Reading" and "Stop". This is the most direct interpretation of a calculated risk!
    /// "Keep Reading" uses the ReadPage function, which uses the current milestone and sends
    /// </summary>
    public class SummoningPhase : Phase
    {
        public GameObject Container;
        
        public Transform PagesTransform;
        public GameObject PagePrefab;
        public Transform PageTurnDestinationTransform;
        
        public override void StartPhase()
        {
            Debug.Log("[SummoningPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            TurnPage();
        }

        public override void EndPhase()
        {
            Debug.Log("[SummoningPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
        }
        
        /// <summary>
        /// Turning a Page causes the PageRead event to fire in GameEvents.cs which mainly adds to PlayerEconomy.
        /// </summary>
        public void TurnPage()
        {
            Debug.Log("[Summoning Phase] Turning the Page...");
            GameObject page = GeneratePage();
            RunPageAnimation(page.GetComponent<SmoothMover>());
            GameEvents.PageRead?.Invoke();
        }

        private GameObject GeneratePage()
        {
            Instantiate(PagePrefab, PagesTransform);
            return Instantiate(PagePrefab, PagesTransform);
        }
        
        private void RunPageAnimation(SmoothMover pageMover)
        {
            pageMover.MoveToTarget(PageTurnDestinationTransform.position, 3.0f, () =>
            {
                Debug.Log("[SummoningPhase] Page Turned!");
            });
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
