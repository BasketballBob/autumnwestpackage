using System.Collections;
using System.Collections.Generic;
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

        private CanvasGroup _canvasGroup;
        private MenuState _currentMenuState;

        public bool IsVisible => _currentMenuState != MenuState.Hidden;

        public enum MenuState { Displayed, Hidden, Entering, Exitting };

        private void Awake()
        {
            _animator?.Play(ExitAnimation, 0, 1);
            _currentMenuState = MenuState.Hidden;
        }

        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1 : 0;
        }

        public void PushSelfInstant()
        {
            PushSelf();
            _animator?.Play(EnterAnimation, 0, 1);
        }

        public void PushSelf()
        {
            BaseGameManager.MenuManager.Push(this);
        }

        public void PopSelf()
        {
            BaseGameManager.MenuManager.Pop(this);
        }

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
