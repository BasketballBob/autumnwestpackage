using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable] [InlineProperty]
    public class AWComparison<TData> : AWCondition
    {
        [HideLabel] [HorizontalGroup("HG")]
        public TData Item1;
        [HideLabel] [HorizontalGroup("HG")]
        public TData Item2;

        public AWComparison() {}

        public override bool Evaluate()
        {
            return Item1.Equals(Item2);
        }
    }
}
