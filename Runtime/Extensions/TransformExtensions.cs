using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class TransformExtensions
    {
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
