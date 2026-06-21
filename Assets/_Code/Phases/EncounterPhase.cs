using System;
using LSG.Core;
using LSG.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Image demonImage;
        
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
            UIEvents.ToggleResourceUI?.Invoke(false);
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
            GameEvents.DiceRollResult?.AddListener(OnDiceRollResult);
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
            // Begin the DICE ROLL!
            GameEvents.DiceRollRequest?.Invoke();
        }

        private void OnDiceRollResult(int d20RollResult)
        {
            if (d20RollResult <= _economy.Rizz)
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
            Debug.Log("[EncounterPhase] You have succeeded the date!");
            UIEvents.ToggleEncounterButtons?.Invoke(false);

            if (_chosenDemonThisPhase.demonName == "Beelzebabe")
            {
                // Look let's just make this easy on ourselves here
                GameEvents.ChangeState?.Invoke(Enums.GameState.WinPhase);
            } else
            {
                UIEvents.SetDialogueText?.Invoke(_chosenDemonThisPhase?.dateOutcome);
            }
        }

        private void FailDate()
        {
            // Give the Boon and Bane Effects and Dialogue
            Debug.Log("[EncounterPhase] You have failed the date!");
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
            demonImage.sprite = _chosenDemonThisPhase.demonSprite;
            demonImage.SetNativeSize();
            GameEvents.DemonEncountered?.Invoke(_chosenDemonThisPhase);
        }
    }
}