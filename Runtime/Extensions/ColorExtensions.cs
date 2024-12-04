using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class ColorExtensions
    {
        public static Color SetR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        public static Color SetG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        public static Color SetB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        public static Color SetA(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }
    }
}
