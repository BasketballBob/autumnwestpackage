using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class SpriteRendererGroup : RendererGroup<SpriteRenderer>
    {
        public AnimatedVar<Color> Color;

        protected override void LateUpdate()
        {
            base.LateUpdate();

            Color.RunOnValueChange(x =>
            {
                ModifyAll(y =>
                {
                    y.color = x;
                });
            });
        }
        
        public override void SyncChildren()
        {
            base.SyncChildren();
            ModifyAll(y =>
            {
                y.color = Color.Value;
            });
        }
    }
}
