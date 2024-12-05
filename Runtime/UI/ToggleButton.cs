using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AWP
{
    [System.Serializable]
    public class ToggleButton : ButtonVariant
    {
        [SerializeField] [ShowIf("ShowToggleValue")]
        protected bool _toggleValue;
        [SerializeField] [ShowIf("ShowUnityEventsInInspector")]
        private UnityEvent _onToggleTrue;
        [SerializeField] [ShowIf("ShowUnityEventsInInspector")]
        private UnityEvent _onToggleFalse;

        protected virtual bool ShowToggleValue => true;
        protected virtual bool ShowUnityEventsInInspector => true;

        protected override void OnPress()
        {
            _toggleValue = !_toggleValue;

            if (_toggleValue) OnToggleTrue();
            else OnToggleFalse();
        }

        protected virtual void OnToggleTrue()
        {
            _onToggleTrue?.Invoke();
        }

        protected virtual void OnToggleFalse()
        {
            _onToggleFalse?.Invoke();
        }
    }
}
