using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class SpriteDecorationTreadmill : DecorationTreadmill<SpriteRenderer>
    {
        [Header("Sprite")]
        [SerializeField] [ShowIf("_useDeltaAnimation")]
        private Gradient _color;

        protected override void ApplyDeltaAnimation(Decoration decor, float delta)
        {
            base.ApplyDeltaAnimation(decor, delta);

            decor.Component.color = _color.Evaluate(delta);
        }
    }
}
