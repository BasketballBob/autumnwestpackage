using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class SpriteRendererGroup : RendererGroup<SpriteRenderer>
    {
        public AnimatedVar<Color> Color;

        [Button]
        private void FlipSortingOrder()
        {
            _items.ForEach(x => x.sortingOrder *= -1);
        }

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
