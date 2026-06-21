using System;
using System.Collections.Generic;
using LSG.Core;
using UnityEngine;

namespace LSG.Utils
{
    class DiceFace
    {
        public string FaceNumber;
        public Transform FaceDirection;

        public DiceFace(string faceNumber, Transform faceDirection)
        {
            FaceNumber = faceNumber;
            FaceDirection = faceDirection;
            
        }
    }
    
    /// <summary>
    /// Really just for the D20.
    /// </summary>
    public class DotProductDetector : MonoBehaviour
    {
        [SerializeField] private bool testRoll = false;

        [SerializeField] private Vector3 dotProductDirection = Vector3.forward;
        [SerializeField] private float tolerance = 0.8f;
        [SerializeField] private GameObject d20;
        [SerializeField] private Transform startPosition;
        [SerializeField] private Vector3 throwForce;

        private List<DiceFace> _faces = new List<DiceFace>();
        
        private Rigidbody _rigidbody;

        private List<GameObject> _selectedGameObjects = new List<GameObject>();
        
        private void OnEnable()
        {
            GameEvents.DiceRollRequest?.AddListener(OnDiceRollRequest);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }

        private void Start()
        {
            CollectFaces();
        }

        private void CollectFaces()
        {
            for (int i = 0; i < d20.transform.childCount; i++)
            {
                Transform child = d20.transform.GetChild(i);
                _faces.Add(new DiceFace(child.name, child));
            }
        }

        private void OnDiceRollRequest()
        {
            ThrowDie();
        }

        public void ThrowDie()
        {
            d20.transform.position = startPosition.position;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(throwForce);
        }

        private void FixedUpdate()
        {
            if (testRoll)
            {
                testRoll = false;
                ThrowDie();
            }
            
            CheckFace();
            /*if (_rigidbody.angularVelocity.magnitude <= 0.005f)
            {
                //Fully stop the die
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.linearVelocity = Vector3.zero;
                
                
            }*/
        }

        private void CheckFace()
        {
            foreach (var face in _faces)
            {
                Vector3 direction = face.FaceDirection.forward - transform.forward;
                float dotProduct = Vector3.Dot(dotProductDirection, direction);

                if (dotProduct > tolerance)
                {
                    _selectedGameObjects.Add(face.FaceDirection.gameObject);
                }
                
                _selectedGameObjects.Add(face.FaceDirection.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            if (_selectedGameObjects.Count != 0) return;
            Gizmos.color = Color.green;
            foreach (var selectedGameObject in _selectedGameObjects)
            {
                Gizmos.DrawSphere(selectedGameObject.transform.position, 1f);
            }
        }
    }
}
