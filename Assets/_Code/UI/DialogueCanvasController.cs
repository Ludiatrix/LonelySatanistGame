using System;
using JetBrains.Annotations;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Controls the visuals, buttons, and text for the dialogue window.
    /// </summary>
    public class DialogueCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text namePlateText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private GameObject summoningButtonContainer;
        [SerializeField] private GameObject encounterButtonContainer;
        [SerializeField] private GameObject storeButtonContainer;

        [Header("Terminal (lose) encounter UI")]
        [SerializeField] private GameObject endgameButtonContainer;
        [SerializeField] private Button tryToDateButton;
        [SerializeField] private TMP_Text giveUpText;

        // True while showing a forced lose-condition encounter (Papiyawn / The Book).
        private bool _terminalEncounter;
        private string _defaultGiveUpText;

        private void Awake()
        {
            if (giveUpText != null)
            {
                _defaultGiveUpText = giveUpText.text;
            }
        }

        private void OnEnable()
        {
            UIEvents.ToggleDialogueWindow?.AddListener(ToggleWindow);
            UIEvents.SetNamePlateText?.AddListener(SetNamePlate);
            UIEvents.SetDialogueText?.AddListener(SetDialogue);
            UIEvents.ToggleSummoningButtons?.AddListener(ToggleSummoningButtonContainer);
            UIEvents.ToggleEncounterButtons?.AddListener(ToggleEncounterButtonContainer);
            UIEvents.AppendDialogueText?.AddListener(AppendDialogue);
            UIEvents.ToggleStoreButtons?.AddListener(ToggleEncounterButtonContainer);
            UIEvents.DisableButtons?.AddListener(DisableButtons);
            UIEvents.FlipDialogueText?.AddListener(OnFlipDialogueText);
            
            // Game-Specific Events for QoL
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
            PhaseEvents.StorePhaseStarted?.AddListener(OnStorePhaseStarted);
            GameEvents.DemonEncountered?.AddListener(OnDemonEncountered);
        }

        private void OnStorePhaseStarted()
        {
            // The dialogue window isn't used during the store; hide it so leftover
            // encounter text/buttons don't bleed through. It's re-shown when the next
            // summoning round or encounter begins.
            ToggleWindow(false);
        }

        private void OnSummoningPhaseStarted()
        {
            // Leaving any terminal-encounter state behind for the new round.
            _terminalEncounter = false;
            ConfigureEncounterButtons();

            ToggleWindow(true);
            SetNamePlate(DataManager.Instance.PlayerEconomySource.PlayerName);
            SetDialogue(string.Empty);
            ToggleSummoningButtonContainer(true);
        }

        private void OnDemonEncountered([CanBeNull] DemonData demonData)
        {
            _terminalEncounter = demonData != null &&
                                 DataManager.Instance.DemonDatingPoolSource.IsForcedEncounterDemon(demonData);

            SetNamePlate(demonData?.demonName);
            SetDialogue(demonData?.introDialogue);
            ToggleWindow(true);
            ToggleEncounterButtonContainer(true);
            ConfigureEncounterButtons();
        }

        /// <summary>
        /// For terminal (lose) encounters the player can't date their way out: the
        /// "Try to Date" button is disabled and "Give Up" is the only way forward.
        /// </summary>
        private void ConfigureEncounterButtons()
        {
            if (tryToDateButton != null)
            {
                tryToDateButton.interactable = !_terminalEncounter;
            }

            if (giveUpText != null)
            {
                giveUpText.text = _terminalEncounter ? "Give Up" : _defaultGiveUpText;
            }
        }

        private void ToggleWindow(bool toggle)
        {
            container.SetActive(toggle);
        }
        
        private void SetNamePlate(string text)
        {
            namePlateText.text = text;
        }
        
        private void SetDialogue(string text)
        {
            dialogueText.text = text;
        }

		private void AppendDialogue(string text)
		{
			dialogueText.text = dialogueText.text + "\n" + text;
		}

        private void ToggleSummoningButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(toggle);
            encounterButtonContainer.SetActive(!toggle);
            storeButtonContainer.SetActive(!toggle);
            SetEndgameContainerActive(false);
        }

        private void ToggleEncounterButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(!toggle);
            encounterButtonContainer.SetActive(toggle);

            // When the encounter buttons are hidden (toggle == false) we're showing
            // the post-encounter "continue" button: normally Store, but End Game for
            // a terminal (Papiyawn / The Book) encounter.
            bool showContinue = !toggle;
            storeButtonContainer.SetActive(showContinue && !_terminalEncounter);
            SetEndgameContainerActive(showContinue && _terminalEncounter);
        }

        private void ToggleStoreButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(!toggle);
            encounterButtonContainer.SetActive(!toggle);
            storeButtonContainer.SetActive(toggle);
            SetEndgameContainerActive(false);
        }

        private void SetEndgameContainerActive(bool active)
        {
            if (endgameButtonContainer != null)
            {
                endgameButtonContainer.SetActive(active);
            }
        }

        private void DisableButtons()
        {
            summoningButtonContainer.SetActive(false);
            encounterButtonContainer.SetActive(false);
            SetEndgameContainerActive(false);
        }
        
        private void OnFlipDialogueText(bool flipX)
        {
            int flipVal = flipX ? -1 : 1;
            
            dialogueText.GetComponent<RectTransform>().localScale = new Vector3(flipVal, 1, 1);
        }
    }
}
