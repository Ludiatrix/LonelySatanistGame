using LSG.Core;
using TMPro;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Tape
    /// </summary>
    public class TapeCountCanvasController : MonoBehaviour
    {
        [SerializeField] private PlayerEconomy playerEconomy;
        [SerializeField] private TMP_Text tapeCountText;

        private void OnEnable()
        {
            GameEvents.PageRead?.AddListener(OnPageRead);
        }

        private void OnDisable()
        {
            GameEvents.PageRead?.RemoveListener(OnPageRead);
        }

        private void OnPageRead()
        {
            tapeCountText.text = $"Tape: {playerEconomy.Tape.ToString()}";
        }
    }
}