using System;
using System.Collections.Generic;
using LSG.Core;
using UnityEngine;

namespace LSG.Utils
{
    [Serializable]
    public class DiceFacePoint
    {
        public string faceNumber;
        public Transform faceTransform;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class D20PointRoller : MonoBehaviour
    {
        [Header("Debug")] [SerializeField] private bool testRoll;
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private bool drawOnlyBestFace;

        [Header("Face Points")] [SerializeField]
        private Transform dieCenter;

        [SerializeField] private Transform faceParent;
        [SerializeField] private List<DiceFacePoint> faces = new();

        [Header("Throw")] [SerializeField] private Transform startPosition;
        [SerializeField] private Transform boardTransform;
        [SerializeField] private float upwardImpulse = 3.5f;
        [SerializeField] private float rightImpulse = 4f;
        [SerializeField] private float forwardVariance = 0.75f;
        [SerializeField] private Vector2 torqueImpulseRange = new(8f, 18f);

        [Header("Result")] [SerializeField] private float minimumRollTime = 0.75f;
        [SerializeField] private float maxRollTime = 8f;
        [SerializeField] private float validFaceDotThreshold = 0.75f;
        [SerializeField] private float settledLinearSpeed = 0.05f;
        [SerializeField] private float settledAngularSpeed = 0.05f;
        [SerializeField] private int settledFrameRequirement = 8;

        [Header("Gizmos")] [SerializeField] private float gizmoRayLength = 0.35f;
        [SerializeField] private float gizmoSphereRadius = 0.04f;

        private Rigidbody _rigidbody;

        private bool _isRolling;
        private bool _hasReportedResult;
        private float _rollStartTime;
        private int _settledFrameCount;

        private DiceFacePoint _currentTopFace;
        private float _currentTopDot;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;

            if (dieCenter == null)
                dieCenter = transform;

            RefreshFacesFromParent();
        }

        private void OnEnable()
        {
            GameEvents.DiceRollRequest?.AddListener(ThrowDie);
        }

        private void OnDisable()
        {
            GameEvents.DiceRollRequest?.RemoveListener(ThrowDie);
        }

        private void FixedUpdate()
        {
            if (testRoll)
            {
                testRoll = false;
                ThrowDie();
            }

            if (_isRolling)
                UpdateRollState();
        }

        [ContextMenu("Refresh Faces From Parent")]
        private void RefreshFacesFromParent()
        {
            faces.Clear();

            if (faceParent == null)
                return;

            for (int i = 0; i < faceParent.childCount; i++)
            {
                Transform child = faceParent.GetChild(i);

                faces.Add(new DiceFacePoint
                {
                    faceNumber = child.name,
                    faceTransform = child
                });
            }
        }

        public void ThrowDie()
        {
            if (!CanThrow())
                return;

            _isRolling = true;
            _hasReportedResult = false;
            _settledFrameCount = 0;
            _currentTopFace = null;
            _currentTopDot = -1f;
            _rollStartTime = Time.time;

            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;

            _rigidbody.position = startPosition.position;
            _rigidbody.rotation = startPosition.rotation;

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.WakeUp();

            Vector3 gravityDirection = GetGravityDirection();
            Vector3 upwardDirection = -gravityDirection;

            Vector3 rightDirection = GetBoardRightDirection(gravityDirection);
            Vector3 forwardDirection = GetBoardForwardDirection(gravityDirection);

            float randomForward = UnityEngine.Random.Range(-forwardVariance, forwardVariance);

            Vector3 impulse =
                upwardDirection * upwardImpulse +
                rightDirection * rightImpulse +
                forwardDirection * randomForward;

            Vector3 torqueAxis = UnityEngine.Random.onUnitSphere;
            float torqueImpulse = UnityEngine.Random.Range(torqueImpulseRange.x, torqueImpulseRange.y);

            _rigidbody.AddForce(impulse, ForceMode.Impulse);
            _rigidbody.AddTorque(torqueAxis * torqueImpulse, ForceMode.Impulse);
        }

        private bool CanThrow()
        {
            if (startPosition == null)
            {
                Debug.LogError("Missing startPosition.", this);
                return false;
            }

            if (dieCenter == null)
            {
                Debug.LogError("Missing dieCenter.", this);
                return false;
            }

            if (faces == null || faces.Count == 0)
            {
                RefreshFacesFromParent();
            }

            if (faces == null || faces.Count == 0)
            {
                Debug.LogError("No dice face points found.", this);
                return false;
            }

            return true;
        }

        private void UpdateRollState()
        {
            float elapsed = Time.time - _rollStartTime;

            if (elapsed < minimumRollTime)
                return;

            if (IsSettled())
                _settledFrameCount++;
            else
                _settledFrameCount = 0;

            bool settledLongEnough = _settledFrameCount >= settledFrameRequirement;
            bool timedOut = elapsed >= maxRollTime;

            if (settledLongEnough || timedOut)
                ReportResult();
        }

        private void ReportResult()
        {
            if (_hasReportedResult)
                return;

            _hasReportedResult = true;
            _isRolling = false;

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.Sleep();

            DiceFacePoint topFace = GetTopFace(out float topDot);

            _currentTopFace = topFace;
            _currentTopDot = topDot;

            if (topFace == null)
            {
                Debug.LogWarning("No top face detected.", this);
                return;
            }

            if (topDot < validFaceDotThreshold)
            {
                Debug.LogWarning($"Best face was {topFace.faceNumber}, but dot was only {topDot:0.000}.", this);
                return;
            }

            Debug.Log($"D20 result: {topFace.faceNumber} | dot: {topDot:0.000}", this);

            GameEvents.DiceRollResult?.Invoke(int.Parse(topFace.faceNumber));
            _rigidbody.useGravity = false;
            _rigidbody.position = startPosition.position;
            _rigidbody.rotation = startPosition.rotation;
        }

        private DiceFacePoint GetTopFace(out float bestDot)
        {
            Vector3 topDirection = GetTopDirection();

            DiceFacePoint bestFace = null;
            bestDot = -1f;

            foreach (DiceFacePoint face in faces)
            {
                if (face == null || face.faceTransform == null)
                    continue;

                Vector3 centerToFace = face.faceTransform.position - dieCenter.position;

                if (centerToFace.sqrMagnitude <= 0.0001f)
                    continue;

                Vector3 faceDirection = centerToFace.normalized;
                float dot = Vector3.Dot(faceDirection, topDirection);

                if (dot > bestDot)
                {
                    bestDot = dot;
                    bestFace = face;
                }
            }

            return bestFace;
        }

        private Vector3 GetTopDirection()
        {
            return -GetGravityDirection();
        }

        private static Vector3 GetGravityDirection()
        {
            if (Physics.gravity.sqrMagnitude <= 0.0001f)
                return Vector3.down;

            return Physics.gravity.normalized;
        }

        private Vector3 GetBoardRightDirection(Vector3 gravityDirection)
        {
            Vector3 right = boardTransform != null ? boardTransform.right : Vector3.right;
            right = Vector3.ProjectOnPlane(right, gravityDirection);

            if (right.sqrMagnitude <= 0.0001f)
                right = Vector3.ProjectOnPlane(Vector3.right, gravityDirection);

            return right.normalized;
        }

        private Vector3 GetBoardForwardDirection(Vector3 gravityDirection)
        {
            Vector3 forward = boardTransform != null ? boardTransform.forward : Vector3.up;
            forward = Vector3.ProjectOnPlane(forward, gravityDirection);

            if (forward.sqrMagnitude <= 0.0001f)
                forward = Vector3.ProjectOnPlane(Vector3.up, gravityDirection);

            return forward.normalized;
        }

        private bool IsSettled()
        {
            return _rigidbody.linearVelocity.sqrMagnitude <= settledLinearSpeed * settledLinearSpeed
                   && _rigidbody.angularVelocity.sqrMagnitude <= settledAngularSpeed * settledAngularSpeed;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos || dieCenter == null || faces == null)
                return;

            DiceFacePoint bestFace = GetTopFace(out _);

            foreach (DiceFacePoint face in faces)
            {
                if (face == null || face.faceTransform == null)
                    continue;

                if (drawOnlyBestFace && face != bestFace)
                    continue;

                Vector3 centerToFace = face.faceTransform.position - dieCenter.position;

                if (centerToFace.sqrMagnitude <= 0.0001f)
                    continue;

                Vector3 faceDirection = centerToFace.normalized;
                bool isBestFace = face == bestFace;

                Gizmos.color = isBestFace ? Color.green : Color.cyan;

                Gizmos.DrawLine(dieCenter.position, face.faceTransform.position);
                Gizmos.DrawSphere(face.faceTransform.position, gizmoSphereRadius);
                Gizmos.DrawRay(face.faceTransform.position, faceDirection * gizmoRayLength);
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(dieCenter.position, GetTopDirection() * 0.75f);
        }
    }
}