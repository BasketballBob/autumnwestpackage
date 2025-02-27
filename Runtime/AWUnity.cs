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
            public static void TraverseSelfAndChildren(this Transform parent, Action<Transform> action)
            {
                action(parent);
                TraverseAllChildren(parent, action);
            }

            public static void TraverseAllChildren(this Transform parent, Action<Transform> action)
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

        #region Misc math
            public static float SignWithZero(float value)
            {
                if (value == 0) return 0;
                else return Mathf.Sign(value);
            }
        #endregion

        #region Colors
            public static Color SetAlpha(this Color color, float alpha)
            {
                return new Color(color.r, color.g, color.b, alpha);
            }
        #endregion

        #region Quaternions

        #endregion

        #region Eulers
            /// <summary>
            /// Gets the shortest rotation to make fromEulerDegrees face the same angle as toEulerDegrees
            /// </summary>
            /// <param name="fromEulerDegrees"></param>
            /// <param name="toEulerDegrees"></param>
            /// <returns></returns>
            public static float GetShortestRotation(float fromEulerDegrees, float toEulerDegrees)
            {
                float difference = Clamp0359(toEulerDegrees) - Clamp0359(fromEulerDegrees);
                if (difference > 180) difference -= 360;
                if (difference < -180) difference += 360;
                //if (difference > 180) difference = 180 - difference;
                //if (difference < -180) difference = 180 + difference;

                return difference;
            }

            public static float Clamp0359(float degrees)
            {
                while (degrees < 0) degrees += 360;
                degrees %= 360;
                return degrees;
            }
        #endregion

        #region Particle Systems
            public static void PlayAndDestroy(this ParticleSystem ps, bool withChildren = true, bool dropParent = true)
            {
                if (dropParent) ps.transform.parent = null;
                ps.Play(withChildren);
                ps.gameObject.AddComponent<AWEmpty>().StartCoroutine(Destroy());

                IEnumerator Destroy()
                {
                    while (ps.IsAlive()) yield return null;
                    GameObject.Destroy(ps.gameObject);
                }
            }
        #endregion

        #region Line Renderer
            public static void PlotTrajectory(this LineRenderer lineRenderer, Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps, Vector2 maxExtents)
            {
                Vector3[] trajectory = AWPhysics2D.PlotTrajectory(rb, pos, velocity, steps, maxExtents).ToVector3Array();
                lineRenderer.positionCount = trajectory.Length;
                lineRenderer.SetPositions(trajectory);
            }

            public static void PlotFunc(this LineRenderer lineRenderer, PlotFunctionData initialData, Action<PlotFunctionData> func, Vector2 pos, int maxSteps, Vector2 maxExtents)
            {
                Vector3[] trajectory = AWPhysics2D.PlotFunc(initialData, func, pos, maxSteps, maxExtents).ToVector3Array();
                lineRenderer.positionCount = trajectory.Length;
                lineRenderer.SetPositions(trajectory);
            }
        #endregion

        #region Conversions
            public static Vector2[] ToVector2Array(this Vector3[] array)
            {
                Vector2[] returnArray = new Vector2[array.Length];
                for (int i = 0; i < returnArray.Length; i++) returnArray[i] = array[i];
                return returnArray;
            }
        #endregion

        #region Debug
        #endregion
    }
}
