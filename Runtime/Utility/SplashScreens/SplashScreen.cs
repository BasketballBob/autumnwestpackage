using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Canvas Canvas => _canvas;
        public RectTransform Rect => _rect;

        public IEnumerator Display()
        {
            _anim.Play("Display");
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
    }
}
