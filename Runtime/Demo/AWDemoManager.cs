using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AWP
{
    public class AWDemoManager : MonoBehaviour
    {
        /// <summary>
        /// Key required to hold to access these commands
        /// </summary>
        [SerializeField]
        private InputActionReference _devKey;
        [SerializeField]
        private InputActionReference _resetKey;

        private void OnEnable()
        {
            _devKey.action.Enable();
            _resetKey.action.Enable();
        }

        private void OnDisable()
        {
            _devKey.action.Disable();
            _resetKey.action.Disable();
        }

        private void Update()
        {
            if (_devKey.action.IsPressed())
            {
                if (_resetKey.action.WasPressedThisFrame()) ResetDemo();
            }
        }

        protected virtual void ResetDemo()
        {
            
        }
    }
}
