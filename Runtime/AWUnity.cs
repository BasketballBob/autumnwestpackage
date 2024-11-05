using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWUnity
    {
        public void TraverseAllChildren(Transform parent, Action<Transform> action)
        {
            RecursiveTraversal(parent);

            void RecursiveTraversal(Transform parent)
            {
                foreach (Transform element in parent)
                {
                    action(element);
                    RecursiveTraversal(parent);
                }
            }
        }
    }
}
