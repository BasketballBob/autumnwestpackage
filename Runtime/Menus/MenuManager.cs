using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.MenuManager)]
    [RequireComponent(typeof(EventSystem))]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Menu _baseMenu;

        private EventSystem _eventSystem;
        private AWStack<MenuItem> _menuStack = new AWStack<MenuItem>();
        private Coroutine _transitionRoutine;
        
        public bool InTransition => _transitionRoutine != null;

        private void OnEnable()
        {
            _eventSystem = GetComponent<EventSystem>();
        }

        private void OnDisable()
        {
            
        }

        public void Push(Menu menu)
        {
            Debug.Log("PUSH COUNT " + _menuStack.Count);
            if (InTransition) return;

            _menuStack.Push(new MenuItem()
            {
                Menu = menu
            });
            
            StartTransitionRoutine(_menuStack.TopItem.Menu.PushAnimation());
        }

        public void Pop()
        {
            Debug.Log("POP COUNT " + _menuStack.Count);
            if (InTransition) return;

            StartTransitionRoutine(_menuStack.TopItem.Menu.PopAnimation());
            _menuStack.Pop();
        }

        public void Pop(Menu menu)
        {
            if (InTransition) return;
            
            for (int i = _menuStack.Count - 1; i > 0; i--)
            {
                if (_menuStack[i].Menu != menu) continue;
                StartTransitionRoutine(_menuStack[i].Menu.PopAnimation());
                _menuStack.RemoveAt(i);
                break;
            }
        }

        public void SetEventSystemEnabled(bool enabled) => _eventSystem.enabled = enabled;

        private void StartTransitionRoutine(IEnumerator routine)
        {
            _transitionRoutine = StartCoroutine(TransitionRoutine());

            IEnumerator TransitionRoutine()
            {
                SetEventSystemEnabled(false);
                yield return routine;
                SetEventSystemEnabled(true);
            }
        }

        private class MenuItem
        {
            public Menu Menu;
        }
    }
}
