using System;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CrossHair : MonoBehaviour
    {
        public Image crossHair;
        public Color enemyColor;
        public Color normalColor;
        private Transform _aimTransform;
        
        public void SetAimTransform(Transform aim)
        {
            _aimTransform = aim;
        }

        private void Update()
        {
            if (Camera.main != null && _aimTransform != null)
            {
                var enemyDetected = Physics.OverlapSphere(_aimTransform.position, 0.001f).Any(e => e.TryGetComponent(typeof(Hitbox), out var c));
                crossHair.color = enemyDetected ? enemyColor : normalColor;
                
                transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(_aimTransform.position), 10 * Time.deltaTime);
            }
        }
    }
}