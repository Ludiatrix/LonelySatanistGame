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

        private void LateUpdate()
        {
            tapeCountText.text = $"{DataManager.Instance.PlayerEconomySource.Tape.ToString()}";
        }
    }
}