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

        [HideInInspector]
        public bool LoopAtEnd = true;
        public Vector2 LoopDistance => (_img.sprite.rect.size - _img.sprite.GetBorderSize()) * (DefaultPixelsPerUnit / _img.sprite.pixelsPerUnit);

        /// <summary>
        /// Call before setting the dimensions
        /// </summary>
        public void SetOffset(Vector3 offset)
        {
            offset = -new Vector3(offset.x % LoopDistance.x, offset.y % LoopDistance.y);
            _rect.localPosition += offset;
        }
    }
}
