using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
    public class RigObject : MonoBehaviour
    {
        private Rig _rig;
        public void Start()
        {
            _rig = GetComponent<Rig>();
        }

        public void Enable()
        {
            _rig.weight = 1;
        }

        public void Disable()
        {
            _rig.weight = 0;
        }
    }
}