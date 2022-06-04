using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Player.Shooting
{
    public class AimState : IStateMachineState<PlayerShoot>
    {
        private float _ammoRemaining;
        public AimState(PlayerShoot playerShoot, int ammo)
        {
            Component = playerShoot;
            _ammoRemaining = ammo;
        }
        public void Update()
        {
            Component.UpdateAimPosition();
        }

        public void OnEnter(IStateMachineState<PlayerShoot> from)
        {
            if (from is ReloadState)
                _ammoRemaining = Component.Gun.ammoCount;
            else if (from is ShootState)
                _ammoRemaining--;
        }

        public void OnLeave()
        {
            
        }

        public Dictionary<int, Func<bool>> Transitions { get; set; }
        public int Id { get; set; }
        public PlayerShoot Component { get; set; }
        public void InitTransitions(StateMachine<PlayerShoot> stateMachine)
        {
            stateMachine.AddTransition<ShootState>(Id, () => Input.GetButton("Fire1") && _ammoRemaining > 0);
            stateMachine.AddTransition<ReloadState>(Id, () => Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Fire1") && _ammoRemaining == 0));
        }
    }
}