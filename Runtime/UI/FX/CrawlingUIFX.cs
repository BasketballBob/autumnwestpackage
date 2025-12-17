using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// UIFX applied to the Crawling Angels main menu
    /// </summary>
    public class CrawlingUIFX : UIFX
    {
        [SerializeField]
        private RectTransform _movedTransform;
        [SerializeField]
        private Vector2 _range = new Vector2(100, 100);
        [SerializeField]
        private float _breathDuration = 4;
        [SerializeField]
        private AnimationCurve _breathCurve;

        private Vector3 _initialScale;
        private float _breathDelta;


        protected override void Start()
        {
            base.Start();

            _initialScale = _movedTransform.localScale;
            StartAnimationRoutines();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_rect.position, _range);
        }

        protected override void FXReset()
        {
            
        }

        protected override void FXUpdate(float deltaTime)
        {
            //ManageBreathing(deltaTime);
        }

        protected override void FXFixedUpdate(float deltaTime)
        {
            
        }

        protected override bool FXFinished()
        {
            return false;
        }

        #region Breathing
        private void ManageBreathing(float deltaTime)
        {
            _breathDelta += deltaTime / _breathDuration;
            _breathDelta %= 1;

            _movedTransform.localScale = _initialScale * _breathCurve.Evaluate(_breathDelta);
        }
        #endregion
    }
}
