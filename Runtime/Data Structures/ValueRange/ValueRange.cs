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
        public RangeMode Mode = RangeMode.TwoConstants;

        [SerializeField]
        private TValue _minConstant;
        [SerializeField]
        private TValue _maxConstant;
        [SerializeField]
        private RandomCurve _randomCurve;

        public ValueRange(TValue min, TValue max)
        {
            _minConstant = min;
            _maxConstant = max;
        }

        public TValue Constant => Min;
        public TValue Min
        {
            get { return _minConstant; }
            set { _minConstant = value; }
        }
        public TValue Max
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

        public TValue GetDelta(TValue value)
        {
            dynamic dynamicValue = value; 
            dynamic min = _minConstant;
            dynamic max = _maxConstant;

            return Mathf.Clamp01((dynamicValue - min) / (max - min));
        }

        public TValue GetReverseDelta(TValue value) => GetReverseValue(GetDelta(value));

        public TValue Lerp(float delta)
        {
            dynamic min = _minConstant;
            dynamic max = _maxConstant;

            return min + (max - min) * delta;
        }
        public TValue ReverseLerp(float delta) => Lerp(Mathf.Clamp01(1 - delta));

        private TValue GetReverseValue(TValue value)
        {
            dynamic one = 1;
            return one - value;
        }
    }

    public class ValueRange
    {
        public enum RangeMode { Constant, TwoConstants, WeightedConstants };
    }
}
