using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [ExecuteInEditMode]
    public abstract class EditorActiveAnimator : MonoBehaviour
    {
        [SerializeField] [Min(.01f)]
        private float _duration = 1;
        [SerializeField]
        private float _enabledDelta = 1;

        private float _timeDelta;

        /// <summary>
        /// Allows animator to be turned on and off in a smooth fashion
        /// </summary>
        public virtual float EnabledDelta { get { return _enabledDelta; } set { _enabledDelta = value; } } 
        protected virtual float Duration => _duration;

        protected virtual void Start()
        {

        }

        protected void Update()
        {
            ManageDelta(Time.deltaTime);
        }

        private void ManageDelta(float deltaTime)
        {
            _timeDelta += deltaTime * (1 / Duration);
            _timeDelta = _timeDelta % 1;

            OnDeltaChange(_timeDelta);
        }

        protected abstract void OnDeltaChange(float delta);
    }
}
