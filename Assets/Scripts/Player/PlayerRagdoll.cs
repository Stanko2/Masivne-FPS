using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
    public class PlayerRagdoll : MonoBehaviour
    {
        private Rigidbody[] _ragdollRigidbodies;
        private Animator _animator;
        private RigBuilder _rigBuilder;

        private void Start()
        {
            Initialize();
            SetRagdollActive(false);
        }

        private void Initialize()
        {
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            _animator = GetComponent<Animator>();
            _rigBuilder = GetComponent<RigBuilder>();
        }

        public void SetRagdollActive(bool active)
        {
            if(_ragdollRigidbodies == null)
                Initialize();
            
            foreach (var ragdollRigidbody in _ragdollRigidbodies)
            {
                ragdollRigidbody.isKinematic = !active;
            }

            _animator.enabled = !active;
            _rigBuilder.enabled = !active;
        }
    }
}