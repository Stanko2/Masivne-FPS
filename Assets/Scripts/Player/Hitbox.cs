using System;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class Hitbox : MonoBehaviour
    {
        public float damageMultiplier;
        private PlayerHealth _playerHealth;

        public int OwningPlayerId => _playerHealth.GetComponent<PlayerController>().owningPlayerId;
        private void Start()
        {
            _playerHealth = GetComponentInParent<PlayerHealth>();
        }

        public bool OnDamage(float damage)
        {
            return _playerHealth.ApplyDamage(damage * damageMultiplier);
        }
    }
}