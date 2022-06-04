using System;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviourPun
    {
        public float maxHealth;
        public event Action<float> OnHealthChange;
        public GameObject deathCam;
        private PlayerController _controller;
        private float _health;
        private PlayerRagdoll _playerRagdoll;

        public void Init()
        {
            _health = maxHealth;
            _controller = GetComponent<PlayerController>();
            Debug.Log("health start");
            _playerRagdoll = GetComponentInChildren<PlayerRagdoll>();
            _playerRagdoll.SetRagdollActive(false);
            if(photonView.IsMine)
                FindObjectOfType<HealthBar>().Initialize(this);
        }

        public void ApplyDamage(float amount)
        {
            _health -= amount;
            Debug.Log(amount);
            _controller.animator.SetTrigger("Hit");
            OnHealthChange?.Invoke(_health);
            if (_health <= 0)
            {
                Dead();
            }
        }

        private void Dead()
        {
            _controller.animator.SetTrigger("Dead");
            _controller.enabled = false;
            GetComponent<PlayerShoot>().enabled = false;
            GetComponent<Collider>().enabled = false;
            _playerRagdoll.SetRagdollActive(true);
            if (photonView.IsMine)
            {
                deathCam.SetActive(true);
                SpawnPlayers.instance.OnPlayerDied();
                Invoke(nameof(Destroy), SpawnPlayers.instance.respawnTime);
            }
        }

        private void Destroy()
        {
            PhotonNetwork.Destroy(_controller.View);
        }
    }
}