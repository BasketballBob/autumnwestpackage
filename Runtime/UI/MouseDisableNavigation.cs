using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace AWP
{
    /// <summary>
    /// Used to prevent mouse from selecting UI when active
    /// Useful for UI that can be navigated with both mouse and the navigation system
    /// </summary>
    public class MouseDisableNavigation : MonoBehaviour
    {
        [SerializeField]
        private EventSystem _eventSystem;
        [SerializeField]
        private InputSystemUIInputModule _input;

        private bool _isMouseControlled;
        private SingleCoroutine _deselectRoutine;

        private void OnEnable() 
        {
            _input.move.action.performed += OnNavigationAction;
            _input.leftClick.action.performed += OnMouseAction;
            
        }

        private void OnDisable()
        {
            _input.move.action.performed -= OnNavigationAction;
            _input.leftClick.action.performed -= OnMouseAction;
        }

        private void Start()
        {
            _deselectRoutine = new SingleCoroutine(this);
        }

        private void Reset()
        {
            _eventSystem = GetComponent<EventSystem>();
            _input = GetComponent<InputSystemUIInputModule>();
        }

        private void OnNavigationAction(InputAction.CallbackContext context)
        {
            _isMouseControlled = false;
            _deselectRoutine.StopRoutine();
        }

        private void OnMouseAction(InputAction.CallbackContext context)
        {
            _isMouseControlled = true;
            _deselectRoutine.StartRoutine(DeselectRoutine());
        }

        private IEnumerator DeselectRoutine()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _eventSystem.SetSelectedGameObject(null);
            }
        }
    }
}
