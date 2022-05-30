using System;
using UnityEngine;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 170f;
        public Transform player;
        public PlayerController controller;
        private float _currRot;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!controller.View.IsMine)
            {
                Destroy(gameObject);
                return;
            }
                
            
            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            
            player.RotateAround(player.position, Vector3.up, rotX * mouseSensitivity * Time.deltaTime);

            _currRot += rotY * mouseSensitivity * Time.deltaTime;
            _currRot = Mathf.Clamp(_currRot, -90f, 90f);
            transform.localRotation = Quaternion.Euler(_currRot, 0,0);
        }
    }
}