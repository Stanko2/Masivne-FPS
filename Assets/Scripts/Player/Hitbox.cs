using System;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class Hitbox : MonoBehaviour, IDamageable
    {
        public float damageMultiplier;
        private PlayerHealth _playerHealth;

        public int OwningPlayerId => _playerHealth.GetComponent<PlayerController>().owningPlayerId;
        private void Start()
        {
            _playerHealth = GetComponentInParent<PlayerHealth>();
        }
        public float Health { get; set; }
        public bool ApplyDamage(float amount)
        {
            return _playerHealth.ApplyDamage(amount * damageMultiplier);
        }

        public void Dead()
        {
            throw new NotImplementedException();
        }
    }
}