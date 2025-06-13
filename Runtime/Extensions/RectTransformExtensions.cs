using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class RectTransformExtensions
    {
        public static Rect GetWorldRect(this RectTransform rect)
        {
            Vector3 maxWorld = rect.TransformPoint(rect.rect.max);
            Vector3 minWorld = rect.TransformPoint(rect.rect.min);

            return new Rect()
            {
                xMax = maxWorld.x,
                xMin = minWorld.x,
                yMax = maxWorld.y,
                yMin = minWorld.y
            };
        }

        public static Vector3 GetWorldTopLeft(this RectTransform rect)
        {
            return rect.TransformPoint(rect.rect.GetTopLeft());
        }

        public static Vector3 GetWorldBottomRight(this RectTransform rect)
        {
            return rect.TransformPoint(rect.rect.GetBottomRight());
        }

        public static void SetRect(this RectTransform rect, Rect newRect)
        {
            rect.localPosition = newRect.center;
            rect.sizeDelta = newRect.size;
        }

        public static void ModifyRect(this RectTransform rect, Func<Rect, Rect> modification)
        {
            Rect newRect = rect.rect;
            rect.SetRect(modification(newRect));
        }
    }
}
