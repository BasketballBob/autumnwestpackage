using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TransformNode : NodeComponent<TransformNode, Transform>
    {
        public override void Initialize()
        {
            Data = transform;
        }
    }
}
