using System;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LSG.Phases
{
    /// <summary>
    /// The Encounter Phase!
    ///
    /// A successful stop rolls an entry on the Demon Encounter Table matching the accumulated power range. The demon introduces themselves via a short interactive dialogue segment.
    /// </summary>
    public class EncounterPhase : Phase
    {
        [SerializeField] private PlayerEconomy playerEconomy;
        [SerializeField] private GameObject Container;

        private void OnEnable()
        {
            GameEvents.TryToDateChosen?.AddListener(OnTryToDateChosen);
            GameEvents.GiveUpChosen?.AddListener(OnGiveUpChosen);
        }

        private void OnDisable()
        {
            GameEvents.TryToDateChosen?.RemoveListener(OnTryToDateChosen);
            GameEvents.GiveUpChosen?.RemoveListener(OnGiveUpChosen);
        }

        private void OnTryToDateChosen()
        {
            GoOnDate();
        }

        private void GoOnDate()
        {
            int d20Roll = Random.Range(1, 21); // The Random D20 Roll for the Date
            if (d20Roll <= playerEconomy.Rizz)
            {
                
            }
        }

        private void OnGiveUpChosen()
        {
            
        }

        public override void StartPhase()
        {
            Debug.Log("[EncounterPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            FindAHottie();
        }

        // Sends the
        private void FindAHottie()
        {
            // Get a demon
            DemonData chosenDemon = playerEconomy.DemonDatingPool.EncounterDemonBasedOnPower(playerEconomy.Power);
            
            // This will turn on the Dialogue Window and inject the DemonData
            GameEvents.DemonEncountered?.Invoke(chosenDemon);
        }
    }
}