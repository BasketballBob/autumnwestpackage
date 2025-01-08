using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AWPEditor
{
    public class ClampedAnimationCurveAttribute : Attribute
    {
        public float MinX = 0;
        public float MaxX = 1;

        public ClampedAnimationCurveAttribute()
        {

        }
    }
}
