using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AWP
{
    public class PauseMenu : Menu
    {
        [SerializeField]
        private InputActionReference _pauseAction;

        [SerializeField]
        private LoopingInstance _snapshotInstance;

        [FoldoutGroup("Events")]
        public UnityEvent OnPause;
        [FoldoutGroup("Events")]
        public UnityEvent OnUnpause;

        protected virtual bool CanPause => true;

        protected override void OnEnable()
        {
            base.OnEnable();

            _pauseAction.action.performed += TryTogglePause;
            //_pauseAction.action.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _pauseAction.action.performed -= TryTogglePause;
            //_pauseAction.action.Disable();
        }

        public void SetPaused(bool paused)
        {
            AWGameManager.SetPaused(paused);
            _snapshotInstance.SetActive(paused);

            if (paused) PushSelf();
            else PopSelf();

            if (paused) OnPause.Invoke();
            else OnUnpause.Invoke();
        }

        public void TryTogglePause(InputAction.CallbackContext context = default)
        {
            if (!CanPause) return;
            if (!_interactable && IsVisible) return;
            if (!AWGameManager.MenuManager.Interactable) return;

            SetPaused(!IsVisible);
        }

        #region Animations
        public override IEnumerator PushAnimation()
        {
            return base.PushAnimation();
        }

        public override IEnumerator PopAnimation()
        {
            return base.PopAnimation();
        }
        #endregion
    }
}
