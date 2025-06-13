using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    public static class SpriteExtensions
    {
        public static Vector2 GetBorderSize(this Sprite sprite)
        {
            return new Vector2(sprite.border.x + sprite.border.z,
                sprite.border.y + sprite.border.w);
        }
    }
}
