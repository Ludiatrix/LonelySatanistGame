using System;
using System.Collections;
using UnityEngine;

namespace LSG.Utils
{
    /// <summary>
    /// Utility class for moving smoothly. Just attach to the object you wanna move.
    /// </summary>
    public class SmoothMover : MonoBehaviour
    {
        // Easing Curve to make the movement feel nice.
        public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        private Coroutine _co = null;

        /// <summary>
        /// Public method to safely trigger the smooth movement.
        /// </summary>
        public void MoveToTarget(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            // Prevent overlapping coroutines from fighting over the position
            if (_co != null)
            {
                StopCoroutine(_co);
            }

            _co = StartCoroutine(MoveOverTimeCoroutine(targetPosition, duration, () =>  onComplete?.Invoke()));
        }

        private IEnumerator MoveOverTimeCoroutine(Vector3 targetPosition, float duration, Action onComplete)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // good ol' fashioned travel code just like mom used to make.
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float curvedTime = movementCurve.Evaluate(normalizedTime);
                transform.position = Vector3.Lerp(startPosition, targetPosition, curvedTime);

                yield return null;
            }

            // Snap it at the end or else it can get all funky.
            transform.position = targetPosition;
            _co = null;
            onComplete?.Invoke();
        }
    }
}