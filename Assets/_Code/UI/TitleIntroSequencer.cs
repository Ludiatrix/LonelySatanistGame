using System;
using System.Collections;
using LSG.Core;
using LSG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        [Header("Background (FadeBlack overlay, fades out to transparent)")]
        [SerializeField] private Image background;
        [SerializeField] private float backgroundFadeSeconds = 1.5f;
        [Tooltip("Invoked after FadeBlack has finished fading out. Wire this to start the game " +
                 "(e.g. StartEmitter.Emit) so the phase change doesn't cut the fade off.")]
        [SerializeField] private UnityEvent onBackgroundFadedOut;

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

            // Game start: (re)enable the black overlay and make it fully opaque. It stays
            // black until the title button is clicked (FadeOutBackground disables it).
            GameObject fadeBlack = FadeBlackObject;
            if (fadeBlack != null) fadeBlack.SetActive(true);
            if (background != null)
                background.color = new Color(0f, 0f, 0f, 1f);
        }

        /// <summary>The FadeBlack overlay object to toggle (the background Image's GameObject).</summary>
        private GameObject FadeBlackObject => background != null ? background.gameObject : null;

        private void PlayIntro()
        {
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(IntroSequence());
        }

        private IEnumerator IntroSequence()
        {
            // The black overlay stays fully opaque through the intro; it only fades out
            // when the title button is clicked (wire OnClick -> FadeOutBackground()).

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

        /// <summary>
        /// Fades the black overlay out to transparent, then disables it. Hook this to the
        /// title button's OnClick so the screen stays black until the player clicks.
        /// </summary>
        public void FadeOutBackground()
        {
            if (background != null) StartCoroutine(FadeBackgroundOut());
        }

        private IEnumerator FadeBackgroundOut()
        {
            Color from = new Color(0f, 0f, 0f, 1f);
            Color to = new Color(0f, 0f, 0f, 0f);
            yield return Lerp(backgroundFadeSeconds, t => background.color = Color.LerpUnclamped(from, to, t));

            // Disable it once transparent so nothing is left covering the screen.
            GameObject fadeBlack = FadeBlackObject;
            if (fadeBlack != null) fadeBlack.SetActive(false);

            // Now that the fade is done, hand off (e.g. start the game / change phase).
            onBackgroundFadedOut?.Invoke();
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
