using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace AWP
{
    public class DebugButton : MonoBehaviour
    {
        public InputActionReference Action;
        public UnityEvent OnPress;

        private void OnEnable()
        {
            Action.action.Enable();
        }

        private void OnDisable()
        {
            Action.action.Disable();
        }

        private void Update()
        {
            if (Action.action.WasPressedThisFrame())
            {
                OnPress?.Invoke();
            }
        }
    }
}
