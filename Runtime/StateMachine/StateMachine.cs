using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class StateMachine
    {
        [SerializeField]
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

        #region Helper functions
        public bool IsCurrentState(BaseState state)
        {
            return _currentState == state;
        }
        #endregion
    }
}
