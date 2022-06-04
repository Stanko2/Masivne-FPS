using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 170f;
        public float rotationSpeed;
        public Transform player;
        public GameObject aimCamera;
        public new GameObject camera;
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
                Destroy(aimCamera);
                Destroy(camera);
                return;
            }
                
            
            var rotX = Input.GetAxis("Mouse X");
            var rotY = Input.GetAxis("Mouse Y");
            player.RotateAround(player.position, Vector3.up, rotX * mouseSensitivity * Time.deltaTime);

            _currRot += rotY * mouseSensitivity * Time.deltaTime;
            _currRot = Mathf.Clamp(_currRot, -70f, 70f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_currRot, 0, 0), rotationSpeed * Time.deltaTime);
        }

        public void AddRecoil(float amount)
        {
            _currRot += amount;
        }
        
        
    }
}