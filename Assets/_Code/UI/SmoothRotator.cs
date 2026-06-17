using System;
using System.Collections;
using UnityEngine;

namespace LSG.Utils
{
    /// <summary>
    /// Utility class for rotating smoothly. Just attach to the object you wanna rotate.
    /// We are operating in 2D space so we can safely use euler angles without fear of gimbal lock.
    /// </summary>
    public class SmoothRotator : MonoBehaviour
    {
        // Easing Curve to make the rotation feel nice.
        public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
        private Coroutine _co = null;

        /// <summary>
        /// Public method to safely trigger the smooth rotation.
        /// </summary>
        public void RotateToTarget(Vector3 targetRotation, float duration, Action onComplete = null)
        {
            // Prevent overlapping coroutines from fighting over the rotation
            if (_co != null)
            {
                StopCoroutine(_co);
            }

            _co = StartCoroutine(RotateOverTimeCoroutine(targetRotation, duration, () =>  onComplete?.Invoke()));
        }

        private IEnumerator RotateOverTimeCoroutine(Vector3 targetRotation, float duration, Action onComplete)
        {
            Vector3 startRotation = transform.eulerAngles;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // good ol' fashioned travel code just like mom used to make.
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float curvedTime = rotationCurve.Evaluate(normalizedTime);
                transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, curvedTime);

                yield return null;
            }

            // Snap it at the end or else it can get all funky.
            transform.eulerAngles = targetRotation;
            _co = null;
            onComplete?.Invoke();
        }
    }
}