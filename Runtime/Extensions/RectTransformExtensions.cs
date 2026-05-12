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

        /// <summary>
        /// Converts a screen position into the local anchored position for rect
        /// Note: transform.position on UI element that are (Screen Space - Overlay) are already screen positions
        /// Reference: https://www.reddit.com/r/Unity3D/comments/ge84w9/best_way_to_convert_from_a_world_position_to_an/
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        public static Vector2 ScreenToAnchoredPosition(this RectTransform rect, Vector2 screenPos)
        {
            return rect.InverseTransformPoint(screenPos);
        }
    }
}
