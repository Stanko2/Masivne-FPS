using System;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviourPun, IDamageable
    {
        public float maxHealth;
        public event Action<float> OnHealthChange;
        public GameObject deathCam;
        private PlayerController _controller;
        public float Health { get; set; }
        private PlayerRagdoll _playerRagdoll;

        public void Init()
        {
            Health = maxHealth;
            _controller = GetComponent<PlayerController>();
            Debug.Log("health start");
            _playerRagdoll = GetComponentInChildren<PlayerRagdoll>();
            _playerRagdoll.SetRagdollActive(false);
            if(photonView.IsMine)
                FindObjectOfType<HealthBar>().Initialize(this);
        }

        public bool ApplyDamage(float amount)
        {
            Health -= amount;
            Debug.Log(amount);
            _controller.animator.SetTrigger("Hit");
            OnHealthChange?.Invoke(Health);
            if (Health <= 0)
            {
                Dead();
                return true;
            }

            return false;
        }

        public void Dead()
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