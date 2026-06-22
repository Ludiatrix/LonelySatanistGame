using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LSG.Utils
{
    /// <summary>
    /// Utility class for rotating smoothly. Just attach to the object you wanna rotate.
    /// We are operating in 2D space so we can safely use euler angles without fear of gimbal lock.
    /// </summary>
    public class SmoothRotator : MonoBehaviour
    {
        [Serializable]
        public class RotationEvent
        {
            [Range(0f, 1f)]
            public float normalizedTime;

            public UnityEvent onReached;
        }

        // Easing Curve to make the rotation feel nice.
        public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Rotation Events")]
        public RotationEvent[] rotationEvents;

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

            _co = StartCoroutine(RotateOverTimeCoroutine(targetRotation, duration, () => onComplete?.Invoke()));
        }

        private IEnumerator RotateOverTimeCoroutine(Vector3 targetRotation, float duration, Action onComplete)
        {
            Vector3 startRotation = transform.eulerAngles;
            float elapsedTime = 0f;

            bool[] firedEvents = new bool[rotationEvents?.Length ?? 0];

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float curvedTime = rotationCurve.Evaluate(normalizedTime);

                transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, curvedTime);

                FireRotationEvents(normalizedTime, firedEvents);

                yield return null;
            }

            transform.eulerAngles = targetRotation;

            // Ensures any event at 1.0 fires.
            FireRotationEvents(1f, firedEvents);

            _co = null;
            onComplete?.Invoke();
        }

        private void FireRotationEvents(float normalizedTime, bool[] firedEvents)
        {
            if (rotationEvents == null)
            {
                return;
            }

            for (int i = 0; i < rotationEvents.Length; i++)
            {
                if (firedEvents[i])
                {
                    continue;
                }

                RotationEvent rotationEvent = rotationEvents[i];

                if (rotationEvent == null)
                {
                    continue;
                }

                if (normalizedTime >= rotationEvent.normalizedTime)
                {
                    firedEvents[i] = true;
                    rotationEvent.onReached?.Invoke();
                }
            }
        }
    }
}