using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public sealed class RendererGroup : RendererGroup<Renderer> { }

    public class RendererGroup<TRenderer> : ComponentGroup<TRenderer> where TRenderer : Renderer
    {
        public void SetEnabled(bool enabled) => ModifyAll(x => x.enabled = enabled);
    }
}
