using System;
using LSG.Core;
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

        private void OnEnable()
        {
            GameEvents.ToggleDialogueWindow?.AddListener(ToggleWindow);
            GameEvents.SetNamePlateText?.AddListener(SetNamePlate);
            GameEvents.SetDialogueText?.AddListener(SetDialogue);
            GameEvents.ToggleSummoningButtons?.AddListener(ToggleSummoningButtonContainer);
            GameEvents.ToggleEncounterButtons?.AddListener(ToggleEncounterButtonContainer);
            GameEvents.DisableButtons?.AddListener(DisableButtons);
        }

        private void ToggleWindow(bool toggle)
        {
            container.SetActive(toggle);
        }
        
        public void SetNamePlate(string text)
        {
            namePlateText.text = text;
        }
        
        public void SetDialogue(string text)
        {
            dialogueText.text = text;
        }

        private void ToggleSummoningButtonContainer(bool toggle)
        {
            summoningButtonContainer.SetActive(toggle);
            encounterButtonContainer.SetActive(!toggle);
        }
        
        private void ToggleEncounterButtonContainer(bool toggle)
        {
            encounterButtonContainer.SetActive(toggle);
            summoningButtonContainer.SetActive(!toggle);
        }

        public void DisableButtons()
        {
            summoningButtonContainer.SetActive(false);
            encounterButtonContainer.SetActive(false);
        }
    }
}
