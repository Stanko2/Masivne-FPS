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
        public float aimSensitivity = 50f;
        public Transform player;
        public GameObject aimCamera;
        public new GameObject camera;
        public PlayerController controller;
        public float minRotX = -70f;
        public float maxRotX = 70f;
        public float minAimRotX = -70f;
        public float maxAimRotX = 50f;
        
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
            if (aimCamera.activeInHierarchy)
            {
                player.RotateAround(player.position, Vector3.up, rotX * aimSensitivity * Time.deltaTime);
                _currRot += rotY * aimSensitivity * Time.deltaTime;
                _currRot = Mathf.Clamp(_currRot, minAimRotX, maxAimRotX);
            }
            else
            {
                player.RotateAround(player.position, Vector3.up, rotX * mouseSensitivity * Time.deltaTime);
                _currRot += rotY * mouseSensitivity * Time.deltaTime;
                _currRot = Mathf.Clamp(_currRot, minRotX, maxRotX);    
            }
            
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_currRot, 0, 0), rotationSpeed * Time.deltaTime);
        }

        public void AddRecoil(float amount)
        {
            _currRot += amount;
        }
        
        
    }
}