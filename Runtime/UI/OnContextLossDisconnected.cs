using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

namespace AWP
{
    /// <summary>
    /// Same thing as OnContextLoss, but uses PointEnterExit to check a different object than the component this one is attached to
    /// </summary>
    public class OnContextLossDisconnected : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _inputAction;
        [SerializeField]
        private PointerEnterExit _enterExit;
        [SerializeField]
        private UnityEvent _onLoss;

        public bool InContext => _enterExit.PointerInside;

        private void Update()
        {
            if (!InContext && _inputAction.action.WasPerformedThisFrame())
            {
                OnLoss();
            }
        }

        private void OnLoss()
        {
            _onLoss.Invoke();
        }
    }
}
