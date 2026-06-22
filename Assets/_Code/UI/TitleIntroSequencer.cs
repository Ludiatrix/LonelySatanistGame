using System;
using System.Collections;
using LSG.Core;
using LSG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSG.UI
{
    /// <summary>
    /// Drives the Title screen intro. When the Title phase starts a black
    /// overlay fades out to transparent, then it fades in the title, the intro narrative,
    /// and the title button one after another (each on its own duration), and
    /// finally disables the title text.
    ///
    /// Put this on the TitlePhase Container (or anything active during the Title
    /// phase) and wire the fade elements / background Image in the inspector.
    /// Each fade element can use a CanvasGroup, a TMP_Text, or both.
    /// </summary>
    public class TitleIntroSequencer : MonoBehaviour
    {
        /// <summary>
        /// A fade-in element. Works with a CanvasGroup (fades the whole subtree),
        /// a TMP_Text (fades just the text), or both at once. Assign whichever
        /// fits the object you're fading.
        /// </summary>
        [Serializable]
        private class FadeTarget
        {
            public CanvasGroup canvasGroup;
            public TMP_Text text;

            [Tooltip("Seconds for this element to fade in.")]
            public float fadeSeconds = 1f;

            public GameObject Owner =>
                canvasGroup != null ? canvasGroup.gameObject :
                text != null ? text.gameObject : null;

            public void SetAlpha(float alpha)
            {
                if (canvasGroup != null) canvasGroup.alpha = alpha;
                if (text != null) text.alpha = alpha;
            }

            public void SetInteractable(bool value)
            {
                if (canvasGroup == null) return;
                canvasGroup.interactable = value;
                canvasGroup.blocksRaycasts = value;
            }

            public void Hide()
            {
                SetAlpha(0f);
                SetInteractable(false);
            }
        }

        [Header("Background (black overlay, fades out to transparent)")]
        [SerializeField] private Image background;
        [SerializeField] private float backgroundFadeSeconds = 1.5f;
        [Tooltip("Disabled once the fade finishes so it stops blocking clicks. Defaults to the background's GameObject.")]
        [SerializeField] private GameObject fadeBlackCanvas;

        [Header("Title")]
        [SerializeField] private FadeTarget title;

        [Header("Intro Narrative")]
        [SerializeField] private FadeTarget introNarrative;
        [Tooltip("Moves the intro narrative up once the title is disabled.")]
        [SerializeField] private SmoothMover introNarrativeMover;
        [SerializeField] private float introNarrativeRiseDistance = 100f;
        [SerializeField] private float introNarrativeRiseDuration = 1f;

        [Header("Title Button")]
        [SerializeField] private FadeTarget titleButton;

        [Tooltip("Optional pause between each element fading in.")]
        [SerializeField] private float delayBetween = 0.1f;

        private Coroutine _routine;

        private void OnEnable()
        {
            ResetToHidden();
            PhaseEvents.TitlePhaseStarted?.AddListener(PlayIntro);
        }

        private void OnDisable()
        {
            PhaseEvents.TitlePhaseStarted?.RemoveListener(PlayIntro);
            if (_routine != null) StopCoroutine(_routine);
        }

        /// <summary>Hide everything up front so nothing flashes before the fades run.</summary>
        private void ResetToHidden()
        {
            title?.Hide();
            introNarrative?.Hide();
            titleButton?.Hide();

            // Start fully black, then fade out to transparent.
            if (background != null)
                background.color = new Color(0f, 0f, 0f, 1f);
        }

        private void PlayIntro()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(IntroSequence());
        }

        private IEnumerator IntroSequence()
        {
            // Black overlay fades out to transparent alongside the rest of the intro.
            if (background != null)
            {
                StartCoroutine(FadeBackgroundOut());
            }

            yield return FadeIn(title);
            yield return Pause();

            // Title fades out smoothly. It stays active (alpha 0) so the layout group
            // doesn't reflow and jump the intro narrative.
            yield return FadeOut(title);

            // Intro narrative fades in next.
            yield return FadeIn(introNarrative);

            // Then slide it up into place, waiting until it's done before continuing.
            SmoothMover mover = ResolveNarrativeMover();
            if (mover != null)
            {
                Vector3 target = mover.transform.position + Vector3.up * introNarrativeRiseDistance;
                bool moveDone = false;
                mover.MoveToTarget(target, introNarrativeRiseDuration, () => moveDone = true);
                yield return new WaitUntil(() => moveDone);
            }
            else
            {
                Debug.LogWarning($"{nameof(TitleIntroSequencer)}: no SmoothMover found for the intro narrative, " +
                                 "so the button won't wait for a move. Assign 'Intro Narrative Mover'.", this);
            }

            yield return Pause();

            // Button shows up last, once the intro narrative is done moving.
            yield return FadeIn(titleButton);
            // Only allow the button to be clicked once it's fully visible.
            titleButton?.SetInteractable(true);
        }

        private IEnumerator FadeBackgroundOut()
        {
            Color from = new Color(0f, 0f, 0f, 1f);
            Color to = new Color(0f, 0f, 0f, 0f);
            yield return Lerp(backgroundFadeSeconds, t => background.color = Color.LerpUnclamped(from, to, t));

            // Disable it once transparent so it stops blocking clicks (e.g. the title button).
            GameObject toDisable = fadeBlackCanvas != null ? fadeBlackCanvas : background.gameObject;
            toDisable.SetActive(false);
        }

        private IEnumerator Pause()
        {
            if (delayBetween > 0f) yield return new WaitForSecondsRealtime(delayBetween);
        }

        /// <summary>
        /// Uses the explicitly assigned mover, falling back to a SmoothMover on the
        /// intro narrative object so the button still waits even if the field is unset.
        /// </summary>
        private SmoothMover ResolveNarrativeMover()
        {
            if (introNarrativeMover != null) return introNarrativeMover;
            if (introNarrative?.Owner != null)
                introNarrativeMover = introNarrative.Owner.GetComponentInParent<SmoothMover>();
            return introNarrativeMover;
        }

        private IEnumerator FadeIn(FadeTarget target)
        {
            if (target == null || target.Owner == null) yield break;
            yield return Lerp(target.fadeSeconds, target.SetAlpha);
        }

        private IEnumerator FadeOut(FadeTarget target)
        {
            if (target == null || target.Owner == null) yield break;
            yield return Lerp(target.fadeSeconds, t => target.SetAlpha(1f - t));
        }

        private IEnumerator Lerp(float seconds, Action<float> apply)
        {
            if (seconds <= 0f)
            {
                apply(1f);
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < seconds)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / seconds);
                t = t * t * (3f - 2f * t); // smoothstep, matches SceneFader
                apply(t);
                yield return null;
            }

            apply(1f);
        }
    }
}
