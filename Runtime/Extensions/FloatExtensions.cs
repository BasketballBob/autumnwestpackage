using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class FloatExtensions
    {
        public static float Lerp(this float val1, float val2, float delta)
        {
            return val1 + (val2 - val1) * Mathf.Clamp01(delta);
        }

        public static float GetDelta(this float value, float min, float max)
        {
            return Mathf.Clamp01((value - min) / (max - min));
        }

        /// <summary>
        /// Gets the Vector that would be created with the degrees of the float
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetAngleVector(this float value)
        {
            return new Vector2(Mathf.Cos(value * Mathf.Deg2Rad), Mathf.Sin(value * Mathf.Deg2Rad));
        }
    }
}
