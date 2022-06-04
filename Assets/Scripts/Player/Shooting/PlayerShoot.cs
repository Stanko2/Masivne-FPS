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
        public LayerMask shootMask;
        
        public Gun Gun { get; private set; }
        public Transform aimTransform;
        private PhotonView _view;
        public Animator animator;

        private int _ammoRemaining;
        private RaycastHit _hit;
        private void Start()
        {
            Gun = Instantiate(gunPrefab, hand).GetComponent<Gun>();
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
            UpdateAimPosition();
        }

        public void UpdateAimPosition()
        {
            aimCamera.SetActive(Input.GetButton("Fire2"));
            if (Physics.Raycast(aimTransform.position, aimTransform.forward, out _hit, float.PositiveInfinity, shootMask))
            {
                aimingTarget.position = _hit.point;
            }
            else
                aimingTarget.position = aimTransform.position + 100 * aimTransform.forward;
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
                {
                    photonView.RPC(nameof(Shoot), RpcTarget.All, aimTransform.position, aimTransform.forward);
                }
                    
                
            }
        }
        
        [PunRPC]
        private void Shoot(Vector3 position, Vector3 direction)
        {
            var effect = Instantiate(Gun.muzzleFlashEffect, Gun.muzzle);
            Destroy(effect, .2f);
            animator.SetTrigger("Shoot");
            GetComponentInChildren<MouseLook>().AddRecoil(-Gun.recoil);
            if (Physics.Raycast(position, direction, out var hit, float.PositiveInfinity ,shootMask))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.TryGetComponent(typeof(Hitbox), out var comp))
                {
                    (comp as Hitbox)?.OnDamage(Gun.damage);
                    
                }
                var o = Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal));
                Destroy(o, 5);
            }
        }
    }
}