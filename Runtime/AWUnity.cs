using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public static class AWUnity
    {
        #region Traversal
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
        #endregion

        #region Events
            public static void AddOneShotListener(this UnityEvent unityEvent, Action action)
            {
                UnityAction oneShotAction = null;
                oneShotAction = () =>
                {
                    action?.Invoke();
                    unityEvent.RemoveListener(oneShotAction);
                };
                unityEvent.AddListener(oneShotAction);
            }
        #endregion

        #region Coroutine
            public static IEnumerator WaitOnRoutines(this MonoBehaviour monoBehaviour, IEnumerator[] enumerators)
            {
                List<Coroutine> routines = new List<Coroutine>();

                for (int i = 0; i < enumerators.Length; i++)
                {
                    if (enumerators[i] == null) continue;
                    routines.Add(monoBehaviour.StartCoroutine(enumerators[i]));
                }

                foreach (Coroutine routine in routines)
                {
                    yield return routine;
                }
            }
        #endregion

        #region Physics
            
        #endregion

        #region Colors
            public static Color ShiftAlpha(Color color, float alpha)
            {
                return new Color(color.r, color.g, color.b, alpha);
            }
        #endregion
    }
}
