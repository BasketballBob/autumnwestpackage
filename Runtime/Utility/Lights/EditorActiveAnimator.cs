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

        private float _timeDelta;

        protected void Update()
        {
            ManageDelta(Time.deltaTime);
        }

        private void ManageDelta(float deltaTime)
        {
            _timeDelta += deltaTime * (1 / _duration);
            _timeDelta = _timeDelta % 1;

            OnDeltaChange(_timeDelta);
        }

        protected abstract void OnDeltaChange(float delta);
    }
}
