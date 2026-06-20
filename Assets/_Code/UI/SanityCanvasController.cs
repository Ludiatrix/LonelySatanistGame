using System.Collections.Generic;
using LSG.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private AnimationCurve sanityFlameOffset;

        [SerializeField] private Image candleImage;

        [Tooltip("Indexed by sanity amount. Start with lowest, end with highest")]
        [SerializeField] private List<Sprite> candleSprites;

        private float initialFlameOffset;

        private void OnEnable()
        {
            GameEvents.PageRead?.AddListener(OnPageRead);
            initialFlameOffset = sanityCandleFlame.transform.localPosition.y;
            ShowResults(20);
        }

        private void OnDisable()
        {
            GameEvents.PageRead?.RemoveListener(OnPageRead);
        }

        private void OnPageRead()
        {
            ShowResults(DataManager.Instance.PlayerEconomySource.Sanity);
        }

        private void ShowResults(int sanity)
        {
            sanityText.text = $"Sanity: {sanity}";

            float scale = sanityFlameSize.Evaluate(sanity);
            sanityCandleFlame.transform.localScale = new Vector3(scale, scale);

            Vector3 flamePosition = sanityCandleFlame.transform.localPosition;
            flamePosition.y = initialFlameOffset + sanityFlameOffset.Evaluate(sanity);
            sanityCandleFlame.transform.localPosition = flamePosition;

            // Set minimum range so sprites don't index oob
            if (sanity < 1) sanity = 1;
            candleImage.sprite = candleSprites[sanity - 1];
        }
    }
}