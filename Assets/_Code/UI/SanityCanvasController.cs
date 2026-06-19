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
        [SerializeField] private GameObject sanityCandleFlame;
        [SerializeField] private AnimationCurve sanityFlameSize;

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
            float scale = sanityFlameSize.Evaluate(DataManager.Instance.PlayerEconomySource.Sanity);
            sanityCandleFlame.transform.localScale = new Vector3(scale, scale);
        }
    }
}