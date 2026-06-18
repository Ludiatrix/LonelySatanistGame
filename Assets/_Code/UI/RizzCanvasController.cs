using LSG.Core;
using TMPro;
using UnityEngine;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Rizz
    /// </summary>
    public class RizzCanvasController : MonoBehaviour
    {
        [SerializeField] private TMP_Text rizzText;
        
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
            rizzText.text = $"Rizz: {DataManager.Instance.PlayerEconomySource.Rizz.ToString()}";
        }
    }
}
