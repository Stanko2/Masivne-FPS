using System;
using System.Collections.Generic;

namespace StateMachine
{
    public interface IStateMachineState<T>
    {
        void Update();
        void OnEnter(IStateMachineState<T> from);
        void OnLeave();
        Dictionary<int, Func<bool>> Transitions { get; set; }
        int Id { get; set; }
        T Component { get; set; }
        void InitTransitions(StateMachine<T> stateMachine);
    }
}