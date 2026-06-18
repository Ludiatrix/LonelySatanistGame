using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSG
{
    /// <summary>
    /// Fades a set of UI objects (e.g. P_StorePage (15)..(24)) to transparent when
    /// the Card Grid slides up, and back when it slides down. Each target gets a
    /// CanvasGroup (added automatically) so its whole hierarchy fades uniformly.
    ///
    /// Wire Hide() to CardDescriptionPanel.onOpened (the slide-up trigger) and
    /// Show() to onClosed.
    /// </summary>
    public class CardGroupFader : MonoBehaviour
    {
        [Header("Targets")]
        [Tooltip("Objects to fade out when the grid slides up (e.g. P_StorePage (15) through (24)).")]
        [SerializeField] private List<GameObject> targets = new();

        [Header("Settings")]
        [Tooltip("Seconds to fade. Match the slide duration; set 0 for an instant change.")]
        [SerializeField] private float fadeDuration = 0.7f;

        [Tooltip("Stop hidden (transparent) objects from receiving clicks while faded out.")]
        [SerializeField] private bool disableRaycastsWhenHidden = true;

        private readonly List<CanvasGroup> groups = new();
        private Coroutine routine;

        private void Awake()
        {
            foreach (GameObject go in targets)
            {
                if (go == null)
                    continue;

                CanvasGroup cg = go.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = go.AddComponent<CanvasGroup>();

                groups.Add(cg);
            }
        }

        /// <summary>Fade the targets out (0% opacity / fully transparent).</summary>
        public void Hide() => FadeTo(0f);

        /// <summary>Fade the targets back in (fully opaque).</summary>
        public void Show() => FadeTo(1f);

        private void FadeTo(float targetAlpha)
        {
            // When hiding, stop the cards blocking/receiving clicks right away.
            if (disableRaycastsWhenHidden && targetAlpha <= 0f)
                SetInteractable(false);

            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(FadeRoutine(targetAlpha));
        }

        private IEnumerator FadeRoutine(float targetAlpha)
        {
            if (fadeDuration > 0f)
            {
                // Capture each group's starting alpha so they animate from where they are.
                int count = groups.Count;
                float[] startAlphas = new float[count];
                for (int i = 0; i < count; i++)
                    startAlphas[i] = groups[i] != null ? groups[i].alpha : targetAlpha;

                float elapsed = 0f;
                while (elapsed < fadeDuration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float t = Mathf.Clamp01(elapsed / fadeDuration);
                    t = t * t * (3f - 2f * t); // smoothstep, matches the slide easing
                    for (int i = 0; i < count; i++)
                        if (groups[i] != null)
                            groups[i].alpha = Mathf.LerpUnclamped(startAlphas[i], targetAlpha, t);
                    yield return null;
                }
            }

            foreach (CanvasGroup cg in groups)
                if (cg != null)
                    cg.alpha = targetAlpha;

            // Restore interaction once fully visible again.
            if (disableRaycastsWhenHidden && targetAlpha > 0f)
                SetInteractable(true);

            routine = null;
        }

        private void SetInteractable(bool value)
        {
            foreach (CanvasGroup cg in groups)
            {
                if (cg == null)
                    continue;
                cg.blocksRaycasts = value;
                cg.interactable = value;
            }
        }
    }
}
