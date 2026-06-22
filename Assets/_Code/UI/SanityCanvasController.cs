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

            // Flip the flame size so it grows as sanity drops, peaking at Sanity = 1.
            // Map sanity [1..max] onto the curve's [max..min] domain (max sanity == candleSprites.Count),
            // so Sanity 1 hits the top of the curve and full sanity hits the bottom.
            int maxSanity = candleSprites.Count;
            int clampedSanity = Mathf.Clamp(sanity, 1, maxSanity);
            float scale = sanityFlameSize.Evaluate(maxSanity + 1 - clampedSanity);
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