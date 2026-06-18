using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Slides a target UI canvas (RectTransform) back down to its original anchored
/// position over a set duration. Triggers when EITHER no button on that canvas
/// is selected, OR a different (purchase) button outside that canvas is selected.
/// SlideDown() is also public so it can be wired directly to a Button's OnClick.
/// </summary>
public class SlideDownWhenDeselected : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The canvas/UI element to slide back down. Set this to the specific canvas you want to control. If left empty, uses this GameObject's RectTransform.")]
    [SerializeField] private RectTransform canvasToSlide;

    [Header("Slide Settings")]
    [Tooltip("Time in seconds the slide takes to complete.")]
    [SerializeField] private float duration = 0.7f;

    private Vector2 originalPosition;
    private bool isDown = true;
    private Coroutine slideRoutine;

    private void Awake()
    {
        if (canvasToSlide == null)
            canvasToSlide = GetComponent<RectTransform>();

        originalPosition = canvasToSlide.anchoredPosition;
    }

    private void Update()
    {
        // Already at the bottom (or sliding there) — nothing to do.
        if (isDown)
            return;

        if (!IsAnythingOnCanvasSelected())
            SlideDown();
    }

    /// <summary>
    /// Call this from your slide-up logic once the canvas has moved up, so this
    /// script knows it now needs to watch for deselection. Without this it
    /// assumes the canvas starts (and stays) down.
    /// </summary>
    public void MarkRaised()
    {
        isDown = false;
    }

    private bool IsAnythingOnCanvasSelected()
    {
        EventSystem es = EventSystem.current;
        if (es == null)
            return false;

        GameObject selected = es.currentSelectedGameObject;
        if (selected == null)
            return false;

        // Selected object counts only if it lives under this canvas.
        return selected.transform.IsChildOf(canvasToSlide);
    }

    /// <summary>
    /// Slides the target canvas back down to its original anchored position.
    /// Public so it can be hooked up to a Button's OnClick event (e.g. a
    /// different purchase button), in addition to the automatic deselection check.
    /// </summary>
    public void SlideDown()
    {
        // Ignore if already down or on the way down.
        if (isDown)
            return;

        isDown = true;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(SlideTo(originalPosition));
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
