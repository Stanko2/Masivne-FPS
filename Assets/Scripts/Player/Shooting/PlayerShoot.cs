using System;
using System.Linq;
using Photon.Pun;
using Player.Shooting;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviourPun
    {
        public event Action<int> OnShoot;
        public GameObject gunPrefab;
        
        public Transform hand;
        public Transform aimingTarget;
        public GameObject aimCamera;
        public GameObject hitEffect;
        public GameObject bullet;
        public LayerMask shootMask;
        
        public Gun Gun { get; private set; }
        public Transform aimTransform;
        private PhotonView _view;
        public Animator animator;

        private int _ammoRemaining;
        private RaycastHit _hit;
        private bool _enabled;
        private PlayerController _controller;
        private void Start()
        {
            Gun = Instantiate(gunPrefab, hand).GetComponent<Gun>();
            _view = GetComponent<PhotonView>();
            _ammoRemaining = Gun.ammoCount;
            _controller = GetComponent<PlayerController>();
            if (_view.IsMine)
            {
                InvokeRepeating(nameof(SendAimPosition), 0, .1f);
                FindObjectOfType<CrossHair>().SetAimTransform(aimingTarget);
                FindObjectOfType<AmmoPanel>().Initialize(this);
            }
        }

        private void SendAimPosition()
        {
            photonView.RPC(nameof(UpdateAim), RpcTarget.Others, aimingTarget.position);
        }

        private float _timer;
        private float _lastShot;
        private void Update()
        {
            if (!_view.IsMine || !_enabled)
                return;
            _timer += Time.deltaTime;
            if (Input.GetButton("Fire"))
                SendShoot();
            UpdateAimPosition();
        }

        public void SetEnabled(bool isEnabled)
        {
            _enabled = isEnabled;
        }
        
        public void UpdateAimPosition()
        {
            aimCamera.SetActive(Input.GetButton("Aim"));
            var hits = Physics.RaycastAll(aimTransform.position, aimTransform.forward, float.PositiveInfinity,
                shootMask);
            _hit = hits.FirstOrDefault(e => !e.collider.transform.IsChildOf(transform));
            if (_hit.collider != null)
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
                OnShoot?.Invoke(_ammoRemaining);
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
                    var hasKill = (comp as Hitbox)?.OnDamage(Gun.damage);
                    if (hasKill == true)
                    {
                        StatsCounter.Instance.OnKill(_controller.owningPlayerId, ((Hitbox) comp).OwningPlayerId);
                    }
                }
                var o = Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal));
                var b = Instantiate(bullet, Gun.muzzle.position, Gun.muzzle.rotation).GetComponent<Bullet>();
                b.TargetPos = hit.point;
                Destroy(o, 5);
            }
        }
    }
}