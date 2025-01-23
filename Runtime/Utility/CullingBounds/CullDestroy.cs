using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class CullDestroy : CullingObject
    {
        public override void Cull()
        {
            Destroy(gameObject);
        }
    }
}
