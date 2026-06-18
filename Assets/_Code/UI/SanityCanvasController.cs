using LSG.Core;
using TMPro;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Sanity
    /// </summary>
    public class SanityCanvasController : MonoBehaviour
    {
        [SerializeField] private TMP_Text sanityText;

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
            sanityText.text = $"Sanity: {DataManager.Instance.PlayerEconomySource.Sanity.ToString()}";
        }
    }
}