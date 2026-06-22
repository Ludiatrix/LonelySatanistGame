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

            int visibleFlowerCount = Mathf.Clamp(rizz, 0, flowers.Count);

            for (var i = 0; i < flowers.Count; i++)
            {
                flowers[i].SetActive(i < visibleFlowerCount);
            }
        }
    }
}
