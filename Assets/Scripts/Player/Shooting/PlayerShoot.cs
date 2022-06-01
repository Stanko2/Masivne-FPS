using System;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviourPun
    {
        public GameObject gunPrefab;
        
        public Transform hand;
        public Transform aimingTarget;
        public GameObject aimCamera;
        public GameObject hitEffect;

        public Gun Gun { get; private set; }
        private Transform _aimTransform;
        private PhotonView _view;
        public Animator animator;

        private int _ammoRemaining;
        
        private void Start()
        {
            Gun = Instantiate(gunPrefab, hand).GetComponent<Gun>();
            _aimTransform = Camera.main.transform;
            _view = GetComponent<PhotonView>();
            _ammoRemaining = Gun.ammoCount;
            if (_view.IsMine)
                InvokeRepeating(nameof(SendAimPosition), 0, .1f);
        }

        private void SendAimPosition()
        {
            photonView.RPC(nameof(UpdateAim), RpcTarget.Others, aimingTarget.position);
        }

        private float _timer;
        private float _lastShot;
        private void Update()
        {
            if (!_view.IsMine)
                return;
            _timer += Time.deltaTime;
            if (Input.GetButton("Fire1"))
                SendShoot();
           
        }

        public void UpdateAimPosition()
        {
            aimCamera.SetActive(Input.GetButton("Fire2"));
            
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            if (Physics.Raycast(_aimTransform.position, _aimTransform.forward, out var hit))
            {
                aimingTarget.position = hit.point;
                Debug.Log(hit.collider.name);
            }
            else
                aimingTarget.position = _aimTransform.position + 100 * _aimTransform.forward;

            gameObject.layer = LayerMask.NameToLayer("Players");
        }
        
        [PunRPC]
        private void UpdateAim(Vector3 aimPos)
        {
            aimingTarget.position = aimPos;
        }

        public void SendShoot()
        {
            if (_timer - _lastShot > Gun.rateOfFire)
            {
                _lastShot = _timer;
                _ammoRemaining--;
                if (_ammoRemaining == -1)
                {
                    animator.SetTrigger("Reload");
                    _lastShot = _timer + Gun.reloadTime;
                    _ammoRemaining = Gun.ammoCount;
                }
                else
                    photonView.RPC(nameof(Shoot), RpcTarget.All);
                
            }
        }
        
        [PunRPC]
        private void Shoot()
        {
            
            var effect = Instantiate(Gun.muzzleFlashEffect, Gun.muzzle.position, Gun.muzzle.rotation);
            Destroy(effect, .2f);
            animator.SetTrigger("Shoot");
            GetComponentInChildren<MouseLook>().AddRecoil(-Gun.recoil);
            if (Physics.Raycast(_aimTransform.position, _aimTransform.forward, out var hit))
            {
                var o = Instantiate(hitEffect, aimingTarget.position, Quaternion.Euler(hit.normal));
                Destroy(o, 5);
                
            }
        }
    }
}