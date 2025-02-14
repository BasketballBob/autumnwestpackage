using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.MenuManager)]
    [RequireComponent(typeof(EventSystem))]
    public class MenuManager : MonoBehaviour
    {
        private EventSystem _eventSystem;
        private InputSystemUIInputModule _uiModule;
        private List<MenuItem> _menuStack = new List<MenuItem>();
        private Coroutine _transitionRoutine;
        
        public bool Interactable => !Disabled && EventSystemEnabled && !InTransition;
        public bool Disabled 
        { 
            get { return _disabled; } 
            set
            {
                _disabled = value;
                SyncEventSystemEnabled();
            }
        }
        private bool _disabled;
        private bool EventSystemEnabled 
        { 
            get { return _eventSystemEnabled; }
            set
            {
                _eventSystemEnabled = value;
                SyncEventSystemEnabled();
            }
        }
        private bool _eventSystemEnabled = true;
        public bool InTransition => _transitionRoutine != null;

        private void OnEnable()
        {
            _eventSystem = GetComponent<EventSystem>();
            _uiModule = GetComponent<InputSystemUIInputModule>();
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            SyncInteractableMenu();
        }

        private void Update()
        {
            //Debug.Log($"Disabled:{Disabled} EventSystemEnabled:{EventSystemEnabled} InTransition:{InTransition} Interactable:{Interactable}");
        }

        public void Push(Menu menu)
        {
            if (!Interactable) return;
            if (StackContainsMenu(menu)) return;

            _menuStack.StackPush(new MenuItem()
            {
                Menu = menu
            });
            SyncInteractableMenu();
            
            StartTransitionRoutine(_menuStack.StackPeek().Menu.PushAnimation());
        }

        public void Pop()
        {
            Pop(_menuStack.StackPeek().Menu);
        }
        public void Pop(Menu menu)
        {
            if (!Interactable) return;
            
            for (int i = _menuStack.Count - 1; i >= 0; i--)
            {
                if (_menuStack[i].Menu != menu) continue;
                StartTransitionRoutine(_menuStack[i].Menu.PopAnimation());
                _menuStack.RemoveAt(i);
                SyncInteractableMenu();
            }
        }

        public IEnumerator WaitOnTransition()
        {
            if (!InTransition) yield break; 
            yield return _transitionRoutine;
        }

        private void SyncEventSystemEnabled()
        {
            bool enabled = !Disabled && EventSystemEnabled;
            
            if (_eventSystem != null) _eventSystem.enabled = enabled;
            if (_uiModule != null) _uiModule.enabled = enabled;
            Debug.Log("EVENTSYS " + _eventSystem.enabled);
        }

        private void SyncInteractableMenu()
        {
            // Manage stack
            for (int i = 0; i < _menuStack.Count; i++)
            {
                if (i == _menuStack.Count - 1)
                {
                    _menuStack[i].Menu.SetInteractable(true);
                }   
                else _menuStack[i].Menu.SetInteractable(false);
            }
        }

        private bool StackContainsMenu(Menu menu)
        {
            for (int i = 0; i < _menuStack.Count; i++)
            {
                if (_menuStack[i].Menu == menu) return true;
            }

            return false;
        }

        private void StartTransitionRoutine(IEnumerator routine)
        {
            _transitionRoutine = StartCoroutine(TransitionRoutine());

            IEnumerator TransitionRoutine()
            {
                EventSystemEnabled = false;
                yield return routine;
                EventSystemEnabled = true;
                _transitionRoutine = null;
            }
        }

        [System.Serializable]
        private class MenuItem
        {
            public Menu Menu;
        }
    }
}
