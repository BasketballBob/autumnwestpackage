using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWDebug
    {
        #region Drawing methods
        public static void DrawRect(Rect rect, Color color, float duration = 0)
        {
            Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);
            Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
            Vector2 topRight = new Vector2(rect.xMax, rect.yMax);
            Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);

            //Debug.Log(topLeft + " " + bottomLeft + " " + topRight + " " + bottomRight);

            Debug.DrawLine(topLeft, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, topRight, color, duration);
            Debug.DrawLine(topRight, topLeft, color, duration);
        }

        public static void DrawRect(Vector2 center, Vector2 size, Color color, float duration = 0)
        {
            Rect rect = new Rect();
            rect.size = size;
            rect.center = center;
            DrawRect(rect, color, duration);
        }
        #endregion
    }
}
