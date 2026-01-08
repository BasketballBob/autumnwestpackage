using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    public static class AWGizmos
    {
        public static void DrawRect(Rect rect)
        {
            Gizmos.DrawWireCube(rect.center, rect.size);
        }

        public static void DrawAngleGizmo(Vector3 position, float minAngle, float maxAngle, float length = 1)
        {
            Gizmos.DrawRay(position, minAngle.GetAngleVector() * length);
            Gizmos.DrawRay(position, maxAngle.GetAngleVector() * length);

            Gizmos.DrawRay(position, (minAngle + AWUnity.GetShortestRotation(minAngle, maxAngle) / 2).GetAngleVector() * length / 2);
        }

        /// <summary>
        /// Draws labels in a list above the positon
        /// </summary>
        /// <param name="position"></param>
        /// <param name="strings"></param>
        public static void DrawLabels(Vector3 position, float separationDist = .25f, params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                Handles.Label(position + Vector3.up * separationDist * (strings.Length - 1 - i), strings[i]);
            }
        }
    }
}
