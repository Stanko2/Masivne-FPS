using System;
using System.Collections;
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

        public void SetRigAimActive(bool active)
        {
            StartCoroutine(SetRigActive(active));
        }

        [SerializeField] private float activeLerpTime = .5f;
        private IEnumerator SetRigActive(bool active)
        {
            var t = 0f;
            while (t < activeLerpTime)
            {
                _rigBuilder.layers[0].rig.weight = active ? t / activeLerpTime : 1 - t / activeLerpTime;
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _rigBuilder.layers[0].rig.weight = active ? 1 : 0;
        }
    }
}