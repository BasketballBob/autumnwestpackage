using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        protected virtual bool CanPause => true;

        protected override void OnEnable()
        {
            base.OnEnable();

            _pauseAction.action.performed += TryTogglePause;
            _pauseAction.action.Enable();
        }

        private void OnDisable()
        {
            _pauseAction.action.performed -= TryTogglePause;
            _pauseAction.action.Disable();
        }

        public void SetPaused(bool paused)
        {
            AWGameManager.SetPaused(paused);
            _snapshotInstance.SetActive(paused);

            if (paused) PushSelf();
            else PopSelf();
        }

        private void TryTogglePause(InputAction.CallbackContext context)
        {
            if (!CanPause) return;
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
