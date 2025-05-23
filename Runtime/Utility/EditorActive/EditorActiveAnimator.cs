using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [ExecuteInEditMode]
    public abstract class EditorActiveAnimator : MonoBehaviour
    {
        [SerializeField] [ShowIf("@ActiveSetting == ActiveType.Duration")]
        [Min(.01f)]
        private float _duration = 1;
        [SerializeField] [ShowIf("@ActiveSetting == ActiveType.Speed")]
        private float _speed = 1;
        [SerializeField] [HideInInspector]
        private float _enabledDelta = 1;

        private float _timeDelta;

        protected enum ActiveType { Default, Duration, Speed }

        /// <summary>
        /// Determines what the deltaTime in OnDeltaChange represents
        /// </summary>
        protected virtual ActiveType ActiveSetting => ActiveType.Default;
        /// <summary>
        /// Allows animator to be turned on and off in a smooth fashion
        /// </summary>
        public virtual float EnabledDelta { get { return _enabledDelta; } set { _enabledDelta = value; } }
        protected virtual float Duration => _duration;
        protected virtual float Speed => _speed;

        protected virtual void Start()
        {

        }

        protected void Update()
        {
            ManageDelta(Time.deltaTime);
        }

        private void ManageDelta(float deltaTime)
        {
            switch (ActiveSetting)
            {
                case ActiveType.Default:
                    OnDeltaChange(deltaTime);
                    break;
                case ActiveType.Duration:
                    _timeDelta += deltaTime * (1 / Duration);
                    _timeDelta = _timeDelta % 1;

                    OnDeltaChange(_timeDelta);
                    break;
                case ActiveType.Speed:
                    OnDeltaChange(deltaTime * _speed);
                    break;
            }
        }

        protected abstract void OnDeltaChange(float delta);
    }
}
