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

        public bool IsVisible => _currentMenuState != MenuState.Hidden;

        public enum MenuState { Displayed, Hidden, Entering, Exitting };

        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            SetMenuState(_defaultState);
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
        }

        public void SetMenuState(MenuState state)
        {
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

        public IEnumerator WaitOnMenuTransition() => AWGameManager.MenuManager.WaitOnTransition();

        public virtual IEnumerator PushAnimation()
        {   
            _currentMenuState = MenuState.Entering;
            SetVisible(true);
            yield return WaitOnTransition(EnterAnimation);
            _currentMenuState = MenuState.Displayed;
            yield break;
        }

        public virtual IEnumerator PopAnimation()
        {
            _currentMenuState = MenuState.Exitting;
            yield return WaitOnTransition(ExitAnimation);
            SetVisible(false);
            _currentMenuState = MenuState.Hidden;
            yield break;
        }

        private IEnumerator WaitOnTransition(string animationName)
        {
            if (_animator == null) yield break;

            _animator.Play(animationName);
            yield return AnimationFX.WaitForAnimationToComplete(_animator);
        }
    }
}
