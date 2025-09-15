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
            Debug.Log($"INVOKE PUSH START {menu.name}");
            menu.OnPushStart?.Invoke();
            StartTransitionRoutine(PushRoutine(menu));
        }
        private IEnumerator PushRoutine(Menu menu)
        {
            //if (!Interactable) return;

            if (!StackContainsMenu(menu))
            {
                StackRemoveMenu(menu);
            }

            _menuStack.StackPush(new MenuItem()
            {
                Menu = menu
            });
            SyncInteractableMenu();

            yield return _menuStack.StackPeek().Menu.PushAnimation();

            menu.OnPushFinish?.Invoke();
        }

        public void Pop()
        {
            Pop(_menuStack.StackPeek().Menu);
        }
        public void Pop(Menu menu)
        {
            menu.OnPopStart?.Invoke();
            StartTransitionRoutine(PopRoutine(menu));
        }
        private IEnumerator PopRoutine(Menu menu)
        {
            //if (!Interactable) return;

            IEnumerator enumerator = null;

            for (int i = _menuStack.Count - 1; i >= 0; i--)
            {
                if (_menuStack[i].Menu != menu) continue;
                enumerator = _menuStack[i].Menu.PopAnimation();
                _menuStack.RemoveAt(i);
                SyncInteractableMenu();
            }

            yield return enumerator;

            menu.OnPopFinish?.Invoke();
        }

        /// <summary>
        /// Pops every menu and except for one that gets pushed
        /// </summary>
        /// <param name="menu"></param>
        public void SoloMenu(Menu menu)
        {
            List<IEnumerator> waitRoutines = new List<IEnumerator>();

            _menuStack.ForEach(x =>
            {
                if (x.Menu == menu) return;
                waitRoutines.Add(PopRoutine(x.Menu));
            });

            waitRoutines.Add(PushRoutine(menu));

            StartTransitionRoutine(this.WaitOnRoutines(waitRoutines.ToArray()));
        }

        public IEnumerator WaitOnTransition()
        {
            Debug.Log($"INTRANSITION " + InTransition);
            if (!InTransition) yield break;
            yield return _transitionRoutine;
        }

        private void SyncEventSystemEnabled()
        {
            bool enabled = !Disabled && EventSystemEnabled;
            
            if (_eventSystem != null) _eventSystem.enabled = enabled;
            if (_uiModule != null) _uiModule.enabled = enabled;
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

        private void StackRemoveMenu(Menu menu)
        {
            for (int i = 0; i < _menuStack.Count; i++)
            {
                if (_menuStack[i].Menu != menu) continue;
                _menuStack.RemoveAt(i);
                i--;
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
            if (_transitionRoutine != null) StopCoroutine(_transitionRoutine);
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
