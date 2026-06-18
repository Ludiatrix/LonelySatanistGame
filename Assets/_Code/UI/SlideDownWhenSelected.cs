using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// A two-way slider for a target canvas (e.g. the Card Grid). Place it on the
/// canvas itself (which must be active at scene start so it captures the correct
/// resting position in Awake). Call SlideUp()/SlideDown() to move the canvas up
/// by 'distance' or back to its original anchored position.
///
/// Intended to be driven by CardDescriptionPanel's events:
///   onOpened -> SlideUp(), onClosed -> SlideDown()
/// so the grid follows the description box opening/closing (including toggle-off).
///
/// Optionally still auto-hooks a targetButton's OnClick to SlideDown (legacy use).
/// </summary>
public class SlideDownWhenSelected : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Optional. If set, this button's OnClick also triggers SlideDown. Leave empty when driving via panel events.")]
    [SerializeField] private Button targetButton;

    [Tooltip("The target canvas/UI element to slide (e.g. the CardGrid). Must be assigned.")]
    [SerializeField] private RectTransform canvasToSlide;

    [Header("Slide Settings")]
    [Tooltip("Time in seconds the slide takes to complete.")]
    [SerializeField] private float duration = 0.7f;

    [Tooltip("Distance in pixels to slide up from the resting position.")]
    [SerializeField] private float distance = 390f;

    private Vector2 originalPosition;
    private Coroutine slideRoutine;

    private void Awake()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        if (canvasToSlide == null)
        {
            Debug.LogError($"{nameof(SlideDownWhenSelected)} on '{name}' has no Canvas To Slide assigned.", this);
            return;
        }

        // Capture the canvas's starting (down) position so we can return to it.
        originalPosition = canvasToSlide.anchoredPosition;
    }

    private void OnEnable()
    {
        if (targetButton != null)
            targetButton.onClick.AddListener(SlideDown);
    }

    private void OnDisable()
    {
        if (targetButton != null)
            targetButton.onClick.RemoveListener(SlideDown);
    }

    /// <summary>
    /// Slides the target canvas up by 'distance' from its resting position.
    /// Wire this to CardDescriptionPanel.onOpened.
    /// </summary>
    public void SlideUp()
    {
        StartSlide(originalPosition + Vector2.up * distance);
    }

    /// <summary>
    /// Slides the target canvas back to its original (resting) anchored position.
    /// Wire this to CardDescriptionPanel.onClosed, or to a close button's OnClick.
    /// </summary>
    public void SlideDown()
    {
        StartSlide(originalPosition);
    }

    private void StartSlide(Vector2 target)
    {
        if (canvasToSlide == null)
            return;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(SlideTo(target));
    }

    private IEnumerator SlideTo(Vector2 target)
    {
        Vector2 from = canvasToSlide.anchoredPosition;
        float elapsed = 0f;

        if (duration <= 0f)
        {
            canvasToSlide.anchoredPosition = target;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // Smoothstep easing to match the slide-up feel.
            t = t * t * (3f - 2f * t);
            canvasToSlide.anchoredPosition = Vector2.LerpUnclamped(from, target, t);
            yield return null;
        }

        canvasToSlide.anchoredPosition = target;
        slideRoutine = null;
    }
}
