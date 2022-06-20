using System;
using System.CodeDom;
using System.Security.AccessControl;
using Unity.Mathematics;
using UnityEngine;

namespace Environment
{
    public class Explosive : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 20f;
        [SerializeField] private GameObject explosionEffectPrefab;
        [SerializeField] private float explosionDamage = 100f;
        [SerializeField] private float explosionRange = 5f;
        [SerializeField] private LayerMask damageMask;
        public float Health { get; set; }

        private void Start()
        {
            Health = maxHealth;
        }

        public bool ApplyDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Dead();
            }

            return Health <= 0;
        }

        public void Dead()
        {
            var effect = Instantiate(explosionEffectPrefab, transform.position, quaternion.identity);
            Destroy(effect, 2);
            var colliders = Physics.OverlapSphere(transform.position, explosionRange, damageMask);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(typeof(IDamageable), out var comp))
                {
                    var rb = comp as IDamageable;
                    var distanceMultiplier = 1-Mathf.InverseLerp(0, explosionRange,
                        Vector3.Distance(transform.position, comp.transform.position));
                    rb.ApplyDamage(explosionDamage * distanceMultiplier);
                }
            }
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(typeof(Rigidbody), out var comp))
                {
                    var rb = comp as Rigidbody;
                    rb.AddExplosionForce(explosionDamage / 10f, transform.position, explosionRange);
                }
            }
            
            Destroy(gameObject);
        }
    }
}