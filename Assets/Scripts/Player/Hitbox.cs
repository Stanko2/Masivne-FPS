using System;
using UnityEngine;

namespace Player
{
    public class Hitbox : MonoBehaviour
    {
        public float damageMultiplier;
        private PlayerHealth _playerHealth;

        private void Start()
        {
            _playerHealth = GetComponentInParent<PlayerHealth>();
        }

        public void OnDamage(float damage)
        {
            _playerHealth.ApplyDamage(damage * damageMultiplier);
        }
    }
}