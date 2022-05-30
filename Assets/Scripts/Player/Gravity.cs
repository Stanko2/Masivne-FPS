using System;
using UnityEngine;

namespace Player
{
    public class Gravity : MonoBehaviour
    {
        public Transform ground;
        public float gravity = Physics.gravity.y;
        public float groundDetectionRadius = .75f;
        public LayerMask groundMask;
        public bool IsGrounded { get; private set; }
        private CharacterController _controller;
        private float _fallVelocity;
        
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_fallVelocity <= 0)
            {
                var objects = Physics.OverlapSphere(ground.position, groundDetectionRadius, groundMask);
                IsGrounded = objects.Length > 0;    
            }
            if (IsGrounded)
                _fallVelocity = 0;
            else
            {
                _fallVelocity += Time.deltaTime * gravity;
                _controller.Move(Vector3.up * (_fallVelocity * Time.deltaTime));
            }
        }

        public void AddVerticalForce(float force)
        {
            Debug.Log(force);
            _fallVelocity = force;
            IsGrounded = false;
        }
    }
}