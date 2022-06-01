using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachine<T>
    {
        public List<IStateMachineState<T>> States;
        public IStateMachineState<T> ActiveState { get; private set; }
        public StateMachine(List<IStateMachineState<T>> states)
        {
            States = new List<IStateMachineState<T>>();
            foreach (var state in states)
            {
                state.Id = States.Count;
                States.Add(state);
            }

            foreach (var state in States)
            {
                state.InitTransitions(this);
            }
        }

        public void Update()
        {
            foreach (var transition in ActiveState.Transitions)
            {
                if (transition.Value())
                {
                    ActiveState.OnLeave();
                    States[transition.Key].OnEnter(ActiveState);
                    ActiveState = States[transition.Key];
                    return;
                }
            }
            ActiveState.Update();
        }

        public void AddTransition<T1>(int from, Func<bool> condition) where T1 : IStateMachineState<T>
        {
            foreach (var state in States.OfType<T1>())
            {
                AddTransitionInternal(from, state.Id, condition);
                break;
            }
        }
        
        private void AddTransitionInternal(int from, int to, Func<bool> condition)
        {
            States[from].Transitions[to] = condition;
        }
    }
}