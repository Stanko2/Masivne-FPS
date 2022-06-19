using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace Player
{
    public class RigController : StateMachineBehaviour
    {

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            var rig = animator.gameObject.GetComponent<PlayerRagdoll>();
            rig.SetRigAimActive(false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            var rig = animator.GetComponent<PlayerRagdoll>();
            rig.SetRigAimActive(true);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}