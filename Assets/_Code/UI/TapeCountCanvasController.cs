using System;
using LSG.Core;
using LSG.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Tape
    /// </summary>
    public class TapeCountCanvasController : MonoBehaviour
    {
        [SerializeField] private TMP_Text tapeCountText;

        private void OnEnable()
        {
            GameEvents.PageRead?.AddListener(OnPageRead);
            GameEvents.CardAdded?.AddListener(OnUpdate);
            GameEvents.CardTaken?.AddListener(OnUpdate);
            CardEvents.BuyCardSuccessResponse?.AddListener(OnUpdate);
        }

        private void OnDisable()
        {
            GameEvents.PageRead?.RemoveListener(OnPageRead);
            GameEvents.CardAdded?.RemoveListener(OnUpdate);
            GameEvents.CardTaken?.RemoveListener(OnUpdate);
            CardEvents.BuyCardSuccessResponse?.RemoveListener(OnUpdate);
        }

        private void OnPageRead()
        {
            tapeCountText.text = $"{DataManager.Instance.PlayerEconomySource.Tape.ToString()}";
        }

        private void OnUpdate(CardData data = null)
        {
            tapeCountText.text = $"{DataManager.Instance.PlayerEconomySource.Tape.ToString()}";
        }
    }
}