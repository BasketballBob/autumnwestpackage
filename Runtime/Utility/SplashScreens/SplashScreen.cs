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
            yield return _anim.WaitForAnimationToComplete("Display");
        }
    }
}
