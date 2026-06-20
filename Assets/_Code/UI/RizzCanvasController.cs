using System.Collections.Generic;
using LSG.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Displays Current Rizz
    /// </summary>
    public class RizzCanvasController : MonoBehaviour
    {
        [SerializeField] private TMP_Text rizzText;

        [SerializeField] private List<GameObject> flowers;
        
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
            int rizz = DataManager.Instance.PlayerEconomySource.Rizz;
            rizzText.text = $"Rizz: {rizz}";
            for (var i = 0; i < 8; i++)
            {
                // I know we can bake the conditional into the call, but that's often harder to read
                if (i >= rizz) flowers[i].SetActive(false);
                else flowers[i].SetActive(true);
            }
        }
    }
}
