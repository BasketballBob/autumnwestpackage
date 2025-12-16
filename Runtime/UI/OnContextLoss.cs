using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AWP
{
    /// <summary>
    /// referencing: https://discussions.unity.com/t/how-to-close-a-ui-panel-when-clicking-outside/578684/10
    /// Balls and gaming
    /// </summary>
    public class OnContextLoss : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// NOTE: INPUT ACTION IS NOT ENABLED OR DISABLED IN THIS SCRIPT
        /// </summary>
        [SerializeField]
        protected InputActionReference _inputAction;
        [SerializeField]
        private UnityEvent _onLoss;

        public bool InContext { get; private set; }

        private void Update()
        {
            //Debug.Log($"INPUT {_inputAction.action.WasPerformedThisFrame()} {_inputAction.action.WasPerformedThisFrame()} {_inputAction.action.IsPressed()} {_inputAction.action.enabled}");
            if (!InContext && _inputAction.action.WasPerformedThisFrame())
            {
                OnLoss();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"CONTEXT ENTER {InContext}");
            InContext = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"CONTEXT EXIT {InContext}");
            InContext = false;
        }

        protected virtual void OnLoss()
        {
            _onLoss.Invoke();
        }
    }
}
