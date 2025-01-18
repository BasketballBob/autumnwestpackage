using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class TransformExtensions
    {
        public static void SetLossyScale(this Transform trans, Vector3 newScale) 
        {
            Vector3 ratio = trans.localScale.CrossFunc(trans.lossyScale, (v1, v2) => v1 / v2);
            trans.localScale = newScale.CrossFunc(ratio, (v1, v2) => v1 * v2);

            Debug.Log($"WEE {newScale} - {trans.lossyScale}");
        }

        public static void UnparentChildren(this Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                trans.GetChild(i).SetParent(null);
                i--;
            }
        }
    }
}
