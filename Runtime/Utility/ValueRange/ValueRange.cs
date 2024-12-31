using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Metadata;

namespace AWP
{
    [System.Serializable]
    public class ValueRange<TValue> : ValueRange where TValue : IComparable
    {
        public RangeMode Mode;

        [SerializeField]
        private TValue _minConstant;
        [SerializeField]
        private TValue _maxConstant;
        [SerializeField]
        private RandomCurve _randomCurve;

        public TValue Constant => MinConstant;
        public TValue MinConstant
        {
            get { return _minConstant; }
            set { _minConstant = value; }
        }
        public TValue MaxConstant
        {
            get { return _maxConstant; }
            set { _maxConstant = value; }
        }


        public TValue Evaluate()
        {
            switch (Mode)
            {
                case RangeMode.Constant:
                    return default;
                case RangeMode.TwoConstants:
                    return Lerp(AWRandom.Range01());
                case RangeMode.WeightedConstants:
                    return Lerp(_randomCurve.Evaluate());
            }

            throw new System.Exception("NO ENUM EXISTS");
        }

        private TValue Lerp(float delta)
        {
            dynamic min = _minConstant;
            dynamic max = _maxConstant;

            return min + (max - min) * delta;
        }
    }

    public class ValueRange
    {
        public enum RangeMode { Constant, TwoConstants, WeightedConstants };
    }
}
