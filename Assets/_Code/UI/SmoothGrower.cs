using System;
using System.Collections;
using UnityEngine;

namespace LSG.Utils
{
    /// <summary>
    /// If you know what I mean.
    /// </summary>
    public class SmoothGrower : MonoBehaviour
    {
        // Easing Curve to make the growing feel nice... lmao
        public AnimationCurve growCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public Vector3 growAmount = Vector3.one;
        public bool returnToNormal = true;
    
        private Coroutine _co = null;

        /// <summary>
        /// Public method to safely trigger the smooth scale.
        /// </summary>
        public void GrowToTarget(Vector3 targetGrow, float duration, Action onComplete = null)
        {
            // Prevent overlapping coroutines from fighting over the scale
            if (_co != null)
            {
                StopCoroutine(_co);
            }

            _co = StartCoroutine(ScaleOverTimeCoroutine(targetGrow, duration, () =>  onComplete?.Invoke()));
        }

        private IEnumerator ScaleOverTimeCoroutine(Vector3 targetScale, float duration, Action onComplete)
        {
            Vector3 startScale = transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // good ol' fashioned travel code just like mom used to make.
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float curvedTime = growCurve.Evaluate(normalizedTime);
                transform.localScale = Vector3.Lerp(startScale, targetScale, curvedTime);

                yield return null;
            }

            // Snap it at the end or else it can get all funky.
            transform.localScale = targetScale;
            
            if (returnToNormal)
            {
                StartCoroutine(ScaleOverTimeCoroutine(startScale, duration, onComplete));
            }
            
            _co = null;
            
            onComplete?.Invoke();
        }
    }
}