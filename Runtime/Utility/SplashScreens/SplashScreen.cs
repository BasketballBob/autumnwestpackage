using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace AWP
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private RectTransform _rect;

        [SerializeField]
        private EventReference _displaySFX;

        private EventInstance _displayInstance;

        public Canvas Canvas => _canvas;
        public RectTransform Rect => _rect;

        private void OnDestroy()
        {
            StopDisplaySFX();
        }

        public IEnumerator Display()
        {
            _anim.Play("Display");
            TryPlayDisplaySFX();
            yield return WaitForDisplayToFinish();
        }

        public IEnumerator WaitForDisplayToFinish()
        {
            yield return _anim.WaitForAnimationToComplete();
        }

        public bool DisplayIsFinished()
        {
            if (_anim == null) return true;
            return _anim.AnimationIsFinished();
        }

        #region SFX
        private void TryPlayDisplaySFX()
        {
            if (_displaySFX.IsNull) return;

            _displayInstance = AWGameManager.AudioManager.CreateInstance(_displaySFX);
            _displayInstance.start();
        }

        private void StopDisplaySFX()
        {
            _displayInstance.DisposeOfSelf();
        }
        #endregion
    }
}
