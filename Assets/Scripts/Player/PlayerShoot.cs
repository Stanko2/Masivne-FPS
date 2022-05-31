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

        private Gun _gun;
        private Transform _aimTransform;
        private PhotonView _view;
        
        private void Start()
        {
            _gun = Instantiate(gunPrefab, hand).GetComponent<Gun>();
            _aimTransform = Camera.main.transform;
            _view = GetComponent<PhotonView>();

            if (_view.IsMine)
                InvokeRepeating(nameof(SendAimPosition), 0, .1f);
        }

        private void SendAimPosition()
        {
            photonView.RPC(nameof(UpdateAim), RpcTarget.Others, aimingTarget.position);
        }
        private void Update()
        {
            if (!_view.IsMine)
                return;
            if (Input.GetButtonDown("Fire1"))
                Shoot();
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
        private void Shoot()
        {
            Debug.Log("Pif!");
        }
    }
}