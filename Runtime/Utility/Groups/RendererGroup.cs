using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public sealed class RendererGroup : RendererGroup<Renderer> { }

    public class RendererGroup<TRenderer> : ComponentGroup<TRenderer> where TRenderer : Renderer
    {
        public AnimatedVar<bool> Enabled = new AnimatedVar<bool>(true);

        protected virtual void LateUpdate()
        {
            Enabled.RunOnValueChange(x =>
            {
                ModifyAll(y => 
                {
                    y.enabled = x;
                });
            });
        }

        public void SetEnabled(bool enabled)
        {
            Enabled.Value = enabled;
        }
    }
}
