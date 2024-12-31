using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// This class roughly creates a weighted random based on some of the concepts from the ziggurat algorithm: https://heliosphan.org/zigguratalgorithm/zigguratalgorithm.html
    /// </summary>
    [System.Serializable]
    public class RandomCurve
    {
        public const float GraphWidth = 1;
        public const float GraphHeight = 1;

        [SerializeField]
        private AnimationCurve _animationCurve = new AnimationCurve();
        [SerializeField]
        private WeightedPool<Tuple<float, float>> _approximationRanges = new WeightedPool<Tuple<float, float>>();
        private int _boxCount = 16;
        private bool _initialized;

        public float Evaluate()
        {
            EnsureInitialized();

            Tuple<float, float> pulledItem = _approximationRanges.PullItem();
            return pulledItem.Item1 + (pulledItem.Item2 - pulledItem.Item1) * AWRandom.Range01();
        }

        private void GenerateApproximation()
        {
            _approximationRanges.Clear();

            float boxStartHeight;
            float boxEndHeight;
            float boxHeight;
            float boxWidth;
            float weight;

            float startX; 
            float endX;

            for (int i = 0; i < _boxCount; i++)
            {
                boxStartHeight = _animationCurve.Evaluate(i / (float)_boxCount);
                boxEndHeight = _animationCurve.Evaluate((i + 1) / (float)_boxCount);
                boxHeight = (boxStartHeight + boxEndHeight) / 2f;
                boxWidth = 1 / (float)_boxCount;
                weight = boxHeight * boxWidth;

                startX = i / (float)_boxCount;
                endX = (i + 1) / (float)_boxCount;

                _approximationRanges.AddItem(new Tuple<float, float>(startX, endX), weight);
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            GenerateApproximation();
            _initialized = true;
        }

        [Button()]
        public void DebugDraw()
        {
            EnsureInitialized();

            for (int i = 0; i < _approximationRanges.Count; i++)
            {
                Vector2 startPos = new Vector2(_approximationRanges[i].Item.Item1, _approximationRanges[i].Weight);
                Vector2 endPos = new Vector2(_approximationRanges[i].Item.Item2, _approximationRanges[i].Weight);

                //Debug.DrawLine(startPos, endPos, Color.red, 10);
                AWDebug.DrawRect(new Rect(startPos.x, 0, endPos.x - startPos.x, _approximationRanges[i].Weight * 10), Color.red);
            }
        }
    }
}
