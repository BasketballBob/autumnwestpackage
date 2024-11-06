using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWUnity
    {
        public static void TraverseAllChildren(Transform parent, Action<Transform> action)
        {
            RecursiveTraversal(parent);

            void RecursiveTraversal(Transform child)
            {
                foreach (Transform element in child)
                {
                    action(element);
                    RecursiveTraversal(element);
                }
            }
        }
    }
}
