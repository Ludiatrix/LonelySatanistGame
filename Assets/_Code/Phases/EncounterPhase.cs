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
        [SerializeField] private GameObject Container;
        
        private PlayerEconomy _economy;
        private DemonData _chosenDemonThisPhase = null;
        
        /// <summary>
        /// Start the Phase and Find a DemonData based on Power in the DemonDatingPool.
        /// </summary>
        public override void StartPhase()
        {
            Debug.Log("[EncounterPhase] Starting Phase!");
            base.StartPhase();
            Container.SetActive(true);
            FindAHottie();
            PhaseEvents.EncounterPhaseStarted?.Invoke();
        }

        public override void EndPhase()
        {
            Debug.Log("[EncounterPhase] Ending Phase!");
            base.EndPhase();
            Container.SetActive(false);
            PhaseEvents.EncounterPhaseEnded?.Invoke();
        }

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

        private void Start()
        {
            _economy = DataManager.Instance.PlayerEconomySource;
        }

        private void OnTryToDateChosen()
        {
            GoOnDate();
        }

        private void GoOnDate()
        {
            int d20Roll = Random.Range(1, 21); // The Random D20 Roll for the Date
            if (d20Roll <= _economy.Rizz)
            {
                SucceedDate();
            }
            else
            {
                FailDate();
            }
        }

        private void OnGiveUpChosen()
        {
            FailDate();
        }

        private void SucceedDate()
        {
            // Date Succeeds!
            Debug.Log("[TryToDateEmitter] You have succeeded the date!");
            UIEvents.ToggleEncounterButtons?.Invoke(false);
            UIEvents.SetDialogueText?.Invoke(_chosenDemonThisPhase?.dateOutcome);
        }

        private void FailDate()
        {
            // Give the Boon and Bane Effects and Dialogue
            Debug.Log("[TryToDateEmitter] You have failed the date!");
            UIEvents.ToggleEncounterButtons?.Invoke(false);
            UIEvents.SetDialogueText?.Invoke(_chosenDemonThisPhase?.boonBaneDialogue);
            _chosenDemonThisPhase?.ApplyEffect();
        }

        // Sends the
        private void FindAHottie()
        {
            // Get a demon
            _chosenDemonThisPhase = DataManager.Instance.DemonDatingPoolSource.EncounterDemonBasedOnPower(_economy.Power);
            
            // This will turn on the Dialogue Window and inject the DemonData
            GameEvents.DemonEncountered?.Invoke(_chosenDemonThisPhase);
        }
    }
}