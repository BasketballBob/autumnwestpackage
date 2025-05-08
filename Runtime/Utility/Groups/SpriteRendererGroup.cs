using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    public class SpriteRendererGroup : RendererGroup<SpriteRenderer>
    {
        public AnimatedVar<Color> Color;

        private void LateUpdate()
        {
            Color.RunOnValueChange(x => 
            {
                ModifyAll(y => y.color = x);
        }   );
        }
    }
}
