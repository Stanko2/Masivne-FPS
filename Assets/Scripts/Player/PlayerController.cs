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
        public PhotonView View { get; private set; }
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _gravity = GetComponent<Gravity>();
            View = GetComponent<PhotonView>();
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
            }
        }
    }
}