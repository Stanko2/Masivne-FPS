using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityTemplateProjects;

namespace Player
{
    public class PlayerController : MonoBehaviourPun
    {
        public float moveSpeed = 5f;
        public float sprintMoveSpeed = 5f;
        public float jumpHeight = 1.5f;
        [NonSerialized] public int owningPlayerId = -1;
        private CharacterController _controller;
        private Gravity _gravity;
        private float _currentMoveSpeed;
        private PlayerShoot _playerShoot;
        public PhotonView View { get; private set; }
        public Animator animator;
        public new GameObject renderer;
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int SpeedX = Animator.StringToHash("SpeedX");
        private static readonly int SpeedY = Animator.StringToHash("SpeedY");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _gravity = GetComponent<Gravity>();
            _playerShoot = GetComponent<PlayerShoot>();
            View = GetComponent<PhotonView>();
            renderer.SetActive(true);
            GetComponent<PlayerHealth>().Init();
            if (View.IsMine)
                photonView.RPC(nameof(RegisterPlayer), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        }

        [PunRPC]
        private void RegisterPlayer(int playerId, string Name)
        {
            StatsCounter.Instance.RegisterPlayer(playerId, Name);
            owningPlayerId = playerId;
        }
        
        private void Update()
        {
            if(!View.IsMine)
                return;
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            _playerShoot.SetEnabled(_gravity.IsGrounded || animator.GetBool(Running));
            
            var move = transform.forward * vertical + transform.right * horizontal;
            _controller.Move(move * (_currentMoveSpeed * Time.deltaTime));
            
            if (Input.GetButtonDown("Jump") && _gravity.IsGrounded)
            {
                _gravity.AddVerticalForce(Mathf.Sqrt(-2f * jumpHeight * _gravity.gravity));
                animator.SetTrigger(Jump);
            }

            if (Input.GetButton("Sprint"))
            {
                animator.SetBool(Running, true);
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, sprintMoveSpeed, 10 * Time.deltaTime);
            }
            else
            {
                animator.SetBool(Running, false);
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, moveSpeed, 10 * Time.deltaTime);
            }
            
            animator.SetFloat(SpeedX, horizontal);
            animator.SetFloat(SpeedY, vertical);
            animator.SetFloat(Speed, move.magnitude);
        }
    }
}