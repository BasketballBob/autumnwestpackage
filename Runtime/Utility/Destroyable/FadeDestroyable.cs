using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
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
            if (_sr != null) _startAlpha = _sr.color.a;
        }

        protected override void DeltaAction(float delta)
        {
            if (_sr != null) _sr.color = _sr.color.SetA(_startAlpha * (1 - delta));
            if (_additionalSpriteRenderers != null)
            {
                _additionalSpriteRenderers?.ForEach(x => 
                {
                    if (x == null) return;
                    x.color = _sr.color;
                });
            }
        }

        public void SetSprites(List<SpriteRenderer> sprites)
        {
            if (sprites.IsNullOrEmpty()) return;
            _sr = sprites[0];
            _startAlpha = _sr.color.a;

            _additionalSpriteRenderers.Clear();
            for (int i = 1; i < sprites.Count; i++)
            {
                _additionalSpriteRenderers.Add(sprites[i]);
            }
        }
    }
}
