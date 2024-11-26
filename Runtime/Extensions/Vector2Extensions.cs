using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.BranchExplorer;
using UnityEngine;
using UnityEngine.Animations;

namespace AWP
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Rounds each axis of the Vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector2 Round(this Vector2 vector, float digits)
        {
            return vector.AxisFunc((x) => AWMath.Round(x));
        }

        /// <summary>
        /// Modifies each axis with the func
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Vector2 AxisFunc(this Vector2 vector, Func<float, float> func)
        {
            return new Vector2(func(vector.x), func(vector.y));
        }

        /// <summary>
        /// Modifies Vector in a Cross Product type fashion, but replaces multiplication of x1 * x2 with provided func
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Vector2 CrossFunc(this Vector2 vector1, Vector2 vector2, Func<float, float, float> func)
        {
            return new Vector2(func(vector1.x, vector2.x), func(vector1.y, vector2.y));
        }

        public static Vector3[] ToVector3Array(this Vector2[] array)
        {
            Vector3[] returnArray = new Vector3[array.Length];
            for (int i = 0; i < returnArray.Length; i++) returnArray[i] = array[i];
            return returnArray;
        }
    }
}
