using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Player.Shooting
{
    public class ReloadState : IStateMachineState<PlayerShoot>
    {
        private static readonly int Reload = Animator.StringToHash("Reload");
        private float _timer;

        public void Update()
        {
            _timer += Time.deltaTime;
        }

        public void OnEnter(IStateMachineState<PlayerShoot> from)
        {
            Component.animator.SetTrigger(Reload);
            _timer = 0;
        }

        public void OnLeave() { }

        public Dictionary<int, Func<bool>> Transitions { get; set; }
        public int Id { get; set; }
        public PlayerShoot Component { get; set; }
        public void InitTransitions(StateMachine<PlayerShoot> stateMachine)
        {
            stateMachine.AddTransition<AimState>(Id, () => _timer >= Component.Gun.reloadTime);
        }
    }
}