using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Base state for the StateMachine
    /// </summary>
    [System.Serializable]
    public abstract class BaseState<TParent> : BaseState
    {
        protected TParent _parent;
        protected TParent P => _parent;

        public BaseState(TParent parent, StateMachine stateMachine) : base(stateMachine)
        {
            _parent = parent;
        }
    }

    [System.Serializable]
    public abstract class BaseState
    {
        protected StateMachine _stateMachine;

        public BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected void SetState(BaseState newState)
        {
            _stateMachine.SetState(newState);
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float fixedDeltaTime) { }
    }
}
