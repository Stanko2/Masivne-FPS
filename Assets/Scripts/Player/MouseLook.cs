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
        public float RotSpeed { get; private set; }

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
            var sensitivity = aimCamera.activeInHierarchy ? aimSensitivity : mouseSensitivity;
            var minX = aimCamera.activeInHierarchy ? minAimRotX : minRotX;
            var maxX = aimCamera.activeInHierarchy ? maxAimRotX : maxRotX;
            player.RotateAround(player.position, Vector3.up, rotX * sensitivity * Time.deltaTime);
            RotSpeed = (new Vector3(rotX, rotY) * sensitivity).magnitude;
            _currRot += rotY * sensitivity * Time.deltaTime;
            _currRot = Mathf.Clamp(_currRot, minX, maxX);
        
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_currRot, 0, 0), rotationSpeed * Time.deltaTime);
        }

        public void AddRecoil(float amount)
        {
            _currRot += amount;
        }
        
        
    }
}