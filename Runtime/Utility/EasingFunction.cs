using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Referenced: https://easings.net/#
    /// </summary>
    [System.Serializable]
    public class EasingFunction
    {
        public static EasingFunction Linear => new EasingFunction(Items[0]);
        public static EasingFunction Sin => new EasingFunction(Items[1]);
        public static EasingFunction EaseInQuint => new EasingFunction(Items[2]);
        public static EasingFunction EaseOutQuint => new EasingFunction(Items[3]);
        public static EasingFunction EaseInExpo => new EasingFunction(Items[4]);

        public static EasingFunctionItem[] Items = new EasingFunctionItem[]
        {
            new EasingFunctionItem("Linear", (x) => x),
            new EasingFunctionItem("Sin", (x) => Mathf.Sin(x * Mathf.PI / 2)),
            new EasingFunctionItem("EasingInQuint", (x) => Mathf.Pow(x, 5)),
            new EasingFunctionItem("EasingOutQuint", (x) => 1 - Mathf.Pow(1 - x, 5)),
            new EasingFunctionItem("EaseInExpo", (x) => Mathf.Pow(2, 10 * x - 10))
        };

        [System.Serializable]
        public delegate float Function(float x);

        public Function Func 
        { 
            get 
            { 
                if (_func != null) return _func;
                return Items[ItemIndex].Func; 
            } 
            set 
            {
                _func = value;
            }
        }
        private Function _func;
        public int ItemIndex;

        private EasingFunction(Function function)
        {
            Func = function;
        }

        private EasingFunction(EasingFunctionItem item)
        {
            Func = item.Func;
        }

        public float GetEasedDelta(float delta)
        {
            return Func(delta);
        }

        public class EasingFunctionItem
        {
            public string Name;
            public Function Func;
    
            public EasingFunctionItem(string name, Function func)
            {
                Name = name;
                Func = func;
            }
        }
    }
}