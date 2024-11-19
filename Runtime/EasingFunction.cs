using System;
using Sirenix.OdinInspector.Editor.Internal;
using UnityEngine;

namespace AWP
{
    public class EasingFunction
    {
        public static EasingFunction Linear => new EasingFunction(LinearFunc);
        public static EasingFunction Sin => new EasingFunction(SinFunc);

        private static Func<float, float> LinearFunc => (x) => x;
        private static Func<float, float> SinFunc => (x) => Mathf.Sin(x * Mathf.PI / 2);

        public Func<float, float> Function = LinearFunc;

        private EasingFunction(Func<float, float> function)
        {
            Function = function;
        }

        public float GetEasedDelta(float delta)
        {
            return Function(delta);
        }
    }
}