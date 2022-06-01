using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Player.Shooting
{
    public class ShootState : IStateMachineState<PlayerShoot>
    {
        private float _timer;
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        public void Update()
        {
            _timer += Time.deltaTime;
        }

        public void OnEnter(IStateMachineState<PlayerShoot> from)
        {
            _timer = 0;
            Component.animator.SetTrigger(Shoot);
            Component.SendShoot();
        }

        public void OnLeave()
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, Func<bool>> Transitions { get; set; }
        public int Id { get; set; }
        public PlayerShoot Component { get; set; }
        public void InitTransitions(StateMachine<PlayerShoot> stateMachine)
        {
            stateMachine.AddTransition<AimState>(Id, () => _timer >= Component.Gun.rateOfFire);
        }
    }
}