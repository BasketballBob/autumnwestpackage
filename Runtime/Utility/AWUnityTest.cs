using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using AWPEditor;
using Sirenix.Serialization;
using UnityEngine.Events;
using Codice.Client.Common;

namespace AWP
{
    public class AWUnityTest : SerializedMonoBehaviour
    {
        // This class is designated for running test code in Unity

        public AWComparison<float> _test = new AWComparison<float>();

        private void LineClosestPointTest()
        {
            Vector2 point = new Vector2(2, 3);
            Func<float, float> line = (x) => 2 * x - 1;
            Vector2 linePoint1 = new Vector2(-100, line(-100));
            Vector2 linePoint2 = new Vector2(100, line(100));

            Debug.Log(AWPhysics2D.LineClosestPoint(linePoint1, linePoint2, point));
        }

        private void RandomCurve()
        {
        
        }

        private void ConditionAllTest()
        {
            List<int> ints = new List<int>() {1};

            Debug.Log($"Any {ints.ConditionAny(x => x == 1)}");
            Debug.Log($"All {ints.ConditionAll(x => x == 1)}");
        }

        public Sprite TestSprite;
    }
}
