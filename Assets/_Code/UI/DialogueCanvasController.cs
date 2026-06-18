using System;
using JetBrains.Annotations;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;

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

        private void OnEnable()
        {
            GameEvents.ToggleDialogueWindow?.AddListener(ToggleWindow);
            GameEvents.SetNamePlateText?.AddListener(SetNamePlate);
            GameEvents.SetDialogueText?.AddListener(SetDialogue);
            GameEvents.ToggleSummoningButtons?.AddListener(ToggleSummoningButtonContainer);
            GameEvents.ToggleEncounterButtons?.AddListener(ToggleEncounterButtonContainer);
            GameEvents.ToggleStoreButtons?.AddListener(ToggleEncounterButtonContainer);
            GameEvents.DisableButtons?.AddListener(DisableButtons);
            
            // Game-Specific Events for QoL
            PhaseEvents.SummoningPhaseStarted?.AddListener(OnSummoningPhaseStarted);
            GameEvents.DemonEncountered?.AddListener(OnDemonEncountered);
        }

        private void OnSummoningPhaseStarted()
        {
            ToggleWindow(true);
            SetNamePlate(DataManager.Instance.PlayerEconomySource.PlayerName);
            SetDialogue(string.Empty);
            ToggleSummoningButtonContainer(true);
        }

        private void OnDemonEncountered([CanBeNull] DemonData demonData)
        {
            SetNamePlate(demonData?.demonName);
            SetDialogue(demonData?.concept);
            ToggleWindow(true);
            ToggleEncounterButtonContainer(true);
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

        private void ToggleSummoningButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(toggle);
            encounterButtonContainer.SetActive(!toggle);
            storeButtonContainer.SetActive(!toggle);
        }
        
        private void ToggleEncounterButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(!toggle);
            encounterButtonContainer.SetActive(toggle);
            storeButtonContainer.SetActive(!toggle);
        }
        
        private void ToggleStoreButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(!toggle);
            encounterButtonContainer.SetActive(!toggle);
            storeButtonContainer.SetActive(toggle);
        }

        private void DisableButtons()
        {
            summoningButtonContainer.SetActive(false);
            encounterButtonContainer.SetActive(false);
        }
    }
}
