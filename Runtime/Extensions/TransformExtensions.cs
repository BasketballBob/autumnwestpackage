using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace AWP
{
    public static class TransformExtensions
    {
        public static void SetLossyScale(this Transform trans, Vector3 newScale)
        {
            if (trans.lossyScale.x == 0 || trans.lossyScale.y == 0 || trans.lossyScale.z == 0) Debug.LogWarning($"LOSSY SCALE ON {trans.name} has an axis of zero");
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

        public static List<Transform> GetChildren(this Transform trans)
        {
            List<Transform> children = new List<Transform>();

            trans.TraverseAllChildren(x =>
            {
                children.AddIfAbsent(x);
            });

            return children;
        }
    }
}
