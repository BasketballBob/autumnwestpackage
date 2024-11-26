using System;
using System.Collections;
using System.Collections.Generic;

namespace AWP
{
    public static class AWMath
    {
        public static float Round(this float value, int digits = 0)
        {
            return (float)Math.Round(value, digits);
        }
    }
}
