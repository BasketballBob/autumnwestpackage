using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class TransformExtensions
    {
        public static void SetLossyScale(this Transform trans, Vector3 newScale)
        {
            Vector3 ratio = trans.localScale.AxisFunc(trans.lossyScale, (v1, v2) => v1 / v2);
            trans.localScale = newScale.AxisFunc(ratio, (v1, v2) => v1 * v2);
        }

        public static void ReparentChildren(this Transform trans, Transform newParent)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                trans.GetChild(i).SetParent(newParent);
                i--;
            }
        }
    }
}
