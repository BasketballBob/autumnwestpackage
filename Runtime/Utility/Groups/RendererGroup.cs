using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class RendererGroup : ComponentGroup<Renderer>
    {
        public void SetEnabled(bool enabled) => ModifyAll(x => x.enabled = enabled);
    }
}
