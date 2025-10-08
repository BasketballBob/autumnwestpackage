using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class RectExtensions
    {
        public static Vector2 GetTopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMax);
        }

        public static Vector2 GetBottomRight(this Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMin);
        }

        public static Vector2 GetRandomPosition(this Rect rect)
        {
            return new Vector2(UnityEngine.Random.Range(rect.min.x, rect.max.x),
                UnityEngine.Random.Range(rect.min.y, rect.max.y));
        }
    }
}
