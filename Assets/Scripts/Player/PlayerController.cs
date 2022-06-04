using System;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 8f;
        public float jumpHeight = 1.5f;
        
        private CharacterController _controller;
        private Gravity _gravity;
        private Vector3 _lastVelocity = Vector3.zero;
        public PhotonView View { get; private set; }
        public Animator animator;
        public new GameObject renderer;
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _gravity = GetComponent<Gravity>();
            View = GetComponent<PhotonView>();
            renderer.SetActive(true);
        }

        private void Update()
        {
            if(!View.IsMine)
                return;
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var move = transform.forward * vertical + transform.right * horizontal;
            _controller.Move(move * (moveSpeed * Time.deltaTime));

            if (Input.GetButtonDown("Jump") && _gravity.IsGrounded)
            {
                _gravity.AddVerticalForce(Mathf.Sqrt(-2f * jumpHeight * _gravity.gravity));
                animator.SetTrigger("Jump");
            }
            
            animator.SetFloat("SpeedX", horizontal);
            animator.SetFloat("SpeedY", vertical);
            animator.SetFloat("Speed", move.magnitude);
            _lastVelocity = _controller.velocity;
        }
    }
}