using System;
using UnityEngine;

namespace Player.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [NonSerialized] public Vector3 TargetPos;
        [SerializeField] private float bulletSpeed;

        private void Start()
        {
            Destroy(gameObject, 5);
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPos, bulletSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, TargetPos) < .1f)
                Destroy(gameObject);
        }
    }
}