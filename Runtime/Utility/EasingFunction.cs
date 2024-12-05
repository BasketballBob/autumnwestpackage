using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Internal;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Referenced: https://easings.net/#
    /// </summary>
    public class EasingFunction
    {
        public static EasingFunction Linear => new EasingFunction(LinearFunc);
        public static EasingFunction Sin => new EasingFunction(SinFunc);
        public static EasingFunction EaseInQuint => new EasingFunction(EaseInQuintFunc);
        public static EasingFunction EaseInExpo => new EasingFunction(EaseInExpoFunc);

        private static Func<float, float> LinearFunc => (x) => x;
        private static Func<float, float> SinFunc => (x) => Mathf.Sin(x * Mathf.PI / 2);
        private static Func<float, float> EaseInQuintFunc => (x) => Mathf.Pow(x, 5);
        private static Func<float, float> EaseInExpoFunc => (x) => Mathf.Pow(2, 10 * x - 10);

        public Func<float, float> Function { get { return _function; } private set { _function = value; }}
        private Func<float, float> _function = LinearFunc;

        private EasingFunction(Func<float, float> function)
        {
            Function = function;
        }

        public float GetEasedDelta(float delta)
        {
            return Function(delta);
        }

        public static IEnumerable GetAll = new ValueDropdownList<EasingFunction>()
        {
            Linear, Sin, EaseInQuint, EaseInExpo
        };
    }
}