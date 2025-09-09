using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.Menu)]
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        protected const string EnterAnimation = "Menu_Enter";
        protected const string ExitAnimation = "Menu_Exit";

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private MenuState _defaultState = MenuState.Hidden;

        protected CanvasGroup _canvasGroup;
        protected MenuState _currentMenuState;
        private bool _skipInitialization;

        public MenuState CurrentMenuState => _currentMenuState;
        public bool IsVisible => _currentMenuState != MenuState.Hidden;
        public bool IsTransitioning => _currentMenuState == MenuState.Entering || _currentMenuState == MenuState.Exitting;

        public enum MenuState { Displayed, Hidden, Entering, Exitting };

        protected virtual void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            if (!_skipInitialization) SetMenuState(_defaultState);
        }

        public void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1 : 0;
        }

        public void SetInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
        }

        public void PushSelf()
        {
            AWGameManager.MenuManager.Push(this);
        }
        public void PushSelfInstant()
        {
            if (_animator == null) return;
            _animator.Play(EnterAnimation, 0, 1);
            PushSelf();
            _currentMenuState = MenuState.Displayed;
        }
        public IEnumerator WaitOnPushSelf()
        {
            PushSelf();
            yield return AWGameManager.MenuManager.WaitOnTransition();
        }

        public void PopSelf()
        {
            AWGameManager.MenuManager.Pop(this);
        }
        public void PopSelfInstant()
        {
            if (_animator == null) return;
            _animator.Play(ExitAnimation, 0, 1);
            PopSelf();  
            _currentMenuState = MenuState.Hidden;
        }
        public IEnumerator WaitOnPopSelf()
        {
            PopSelf();
            yield return AWGameManager.MenuManager.WaitOnTransition();
        }

        public void SoloSelf()
        {
            AWGameManager.MenuManager.SoloMenu(this);
        }
        public IEnumerator WaitOnSoloSelf()
        {
            SoloSelf();
            yield return AWGameManager.MenuManager.WaitOnTransition();
        }

        public void SetMenuState(MenuState state)
        {
            Debug.Log($"VIDEO {gameObject} {state}");
            switch (state)
            {
                case MenuState.Displayed:
                    PushSelfInstant();
                    break;
                case MenuState.Hidden:
                    PopSelfInstant();
                    break;
                case MenuState.Entering:
                    PushSelf();
                    break;
                case MenuState.Exitting:
                    PopSelf();
                    break;
            }
        }

        public void SkipInitialization() => _skipInitialization = true;
        public IEnumerator WaitOnMenuTransition() => AWGameManager.MenuManager.WaitOnTransition();

        public virtual IEnumerator PushAnimation()
        {   
            Debug.Log("PUSH START " + gameObject.name + " " + _currentMenuState);
            _currentMenuState = MenuState.Entering;
            SetVisible(true);
            yield return WaitOnTransition(EnterAnimation);
            _currentMenuState = MenuState.Displayed;
            Debug.Log("PUSH STOP " + gameObject.name + " " + _currentMenuState);
            yield break;
        }

        public virtual IEnumerator PopAnimation()
        {
            Debug.Log("POP START " + gameObject.name + " " + _currentMenuState);
            _currentMenuState = MenuState.Exitting;
            yield return WaitOnTransition(ExitAnimation);
            SetVisible(false);
            _currentMenuState = MenuState.Hidden;
            Debug.Log("POP STOP " + gameObject.name + " " + _currentMenuState);
            yield break;
        }

        private IEnumerator WaitOnTransition(string animationName)
        {
            if (_animator == null) yield break;

            _animator.Play(animationName);
            yield return _animator.WaitForAnimationToComplete();
        }
    }
}
