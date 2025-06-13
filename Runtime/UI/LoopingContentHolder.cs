using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class LoopingContentHolder : ContentHolder
    {
        private const float DefaultPixelsPerUnit = 100;
        [SerializeField]
        private Image _img;

        public bool ShouldLoop { get; set; }
        public bool LoopAtEnd { get; set; }
        public Vector2 LoopDistance => (_img.sprite.rect.size - _img.sprite.GetBorderSize()) * (DefaultPixelsPerUnit / 32);

        /// <summary>
        /// Call after setting the dimensions
        /// </summary>
        public void SetOffset(Vector2 offset)
        {
            //if (!ShouldLoop) return;
            if (ShouldLoop) _rect.localPosition -= new Vector3(offset.x % LoopDistance.x, offset.y % LoopDistance.y);
            else _rect.localPosition -= new Vector3(offset.x, offset.y);
        }

        protected override void SetRect(Rect rect)
        {
            // _rect.SetRect(rect);
            // return;

            Vector2 contentSize = rect.size - _img.sprite.GetBorderSize();
            Vector2 newSize = contentSize.SetY(LoopDistance.y * Mathf.Ceil(contentSize.y / LoopDistance.y));
            Vector2 sizeChange = newSize - rect.size;

            if (LoopAtEnd)
            {
                rect.yMax += sizeChange.y; // * (ShouldLoop ? 1 : -1);
            }
            else rect.yMin -= sizeChange.y; // * (ShouldLoop ? 1 : -1);
            
            
            _rect.SetRect(rect);
        }
    }
}
