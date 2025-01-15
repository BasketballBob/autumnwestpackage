using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class FadeDestroyable : Destroyable
    {
        [SerializeField]
        private SpriteRenderer _sr;

        private float _startAlpha;

        private void Start()
        {
            _startAlpha = _sr.color.a;
        }

        protected override void DeltaAction(float delta)
        {
            _sr.color = _sr.color.SetA(_startAlpha * (1 - delta));
        }
    }
}
