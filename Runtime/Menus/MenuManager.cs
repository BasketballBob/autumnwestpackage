using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.MenuManager)]
    [RequireComponent(typeof(EventSystem))]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Menu _baseMenu;

        private EventSystem _eventSystem;
        private InputSystemUIInputModule _uiModule;
        private AWStack<MenuItem> _menuStack = new AWStack<MenuItem>();
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

        public void Push(Menu menu)
        {
            if (!Interactable) return;

            _menuStack.Push(new MenuItem()
            {
                Menu = menu
            });
            
            StartTransitionRoutine(_menuStack.TopItem.Menu.PushAnimation());
        }

        public void Pop()
        {
            if (!Interactable) return;

            StartTransitionRoutine(_menuStack.TopItem.Menu.PopAnimation());
            _menuStack.Pop();
        }

        public void Pop(Menu menu)
        {
            if (!Interactable) return;
            
            for (int i = _menuStack.Count - 1; i >= 0; i--)
            {
                if (_menuStack[i].Menu != menu) continue;
                StartTransitionRoutine(_menuStack[i].Menu.PopAnimation());
                _menuStack.RemoveAt(i);
                break;
            }
        }

        private void SyncEventSystemEnabled()
        {
            bool enabled = !Disabled && EventSystemEnabled;
            
            if (_eventSystem != null) _eventSystem.enabled = enabled;
            if (_uiModule != null) _uiModule.enabled = enabled;
            Debug.Log("EVENTSYS " + _eventSystem.enabled);
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

        private class MenuItem
        {
            public Menu Menu;
        }
    }
}
