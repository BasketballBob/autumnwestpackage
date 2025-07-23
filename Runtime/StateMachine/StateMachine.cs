using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class StateMachine
    {
        private BaseState _currentState;

        public void SetState(BaseState newState)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState?.EnterState();
        }   

        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }
        
        public void FixedUpdate(float fixedDeltaTime)
        {
            _currentState?.FixedUpdate(fixedDeltaTime);
        }
    }
}
