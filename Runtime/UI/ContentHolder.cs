using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    public class ContentHolder : AWUI
    {
        [SerializeField]
        protected RectTransform _rect;
        [SerializeField] // Left top right bottom
        private Vector4 _margins;

        public RectTransform Rect => _rect;
        public Vector2 MarginSize => new Vector2(_margins.x + _margins.z, _margins.y + _margins.w);
        public float LeftMargin => _margins.x;
        public float TopMargin => _margins.y;
        public float RightMargin => _margins.z;
        public float BottomMargin => _margins.w;
        public Vector2 ContentSize => _rect.sizeDelta - MarginSize;

        private void Reset()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void OnDrawGizmos()
        {
            if (_rect == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;

            Rect marginRect = _rect.rect;
            marginRect.xMin += _margins.x;
            marginRect.yMax -= _margins.y;
            marginRect.xMax -= _margins.z;
            marginRect.yMin += _margins.w;
            Gizmos.DrawWireCube(marginRect.center, marginRect.size);
        }

        // /// <summary>
        // /// Clockwise starting at bottom left
        // /// </summary>
        // /// <param name="fourCornersArray"></param>
        // public void FitToContent(Vector3 topLeft, Vector3 bottomRight)
        // {
        //     Vector3 localTopLeft = transform.InverseTransformPoint(topLeft);
        //     Vector3 localBottomRight = transform.InverseTransformPoint(bottomRight);

        //     Rect newRect = _rect.rect;
        //     newRect.xMin = localTopLeft.x;
        //     newRect.yMax = localTopLeft.y;
        //     newRect.xMax = localBottomRight.x;
        //     newRect.yMin = localBottomRight.y;

        //     _rect.localPosition = newRect.center;
        //     _rect.sizeDelta = newRect.size;
        // }

        public void FitToHeightWorld(Vector3 worldTop, Vector3 worldBottom)
        {
            Vector3 localTop = transform.parent.InverseTransformPoint(worldTop);
            Vector3 localBottom = transform.parent.InverseTransformPoint(worldBottom);

            FitToHeight(localTop, localBottom);
        }

        public void FitToHeight(Vector2 top, Vector2 bottom)
        {
            Rect newRect = _rect.rect;
            newRect.yMax = top.y + TopMargin;
            newRect.yMin = bottom.y - BottomMargin;

            SetRect(newRect);
        }

        protected virtual void SetRect(Rect rect)
        {
            _rect.SetRect(rect);
        }
    }
}
