using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class FadeDestroyable : Destroyable
    {
        [SerializeField]
        private SpriteRenderer _sr;
        [SerializeField]
        private List<SpriteRenderer> _additionalSpriteRenderers = new List<SpriteRenderer>();

        private float _startAlpha;

        private void Start()
        {
            _startAlpha = _sr.color.a;
        }

        protected override void DeltaAction(float delta)
        {
            _sr.color = _sr.color.SetA(_startAlpha * (1 - delta));
            if (_additionalSpriteRenderers != null)
            {
                _additionalSpriteRenderers?.ForEach(x => 
                {
                    if (x == null) return;
                    x.color = _sr.color;
                });
            }
        }
    }
}
