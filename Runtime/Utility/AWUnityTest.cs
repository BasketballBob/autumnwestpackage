using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace AWP
{
    public class AWUnityTest : MonoBehaviour
    {
        /// This class is designated for running test code in Unity

        [Button("Test")]
        private void Start()
        {
            RoundTest();
        }

        private void LineClosestPointTest()
        {
            Vector2 point = new Vector2(2, 3);
            Func<float, float> line = (x) => 2 * x - 1;
            Vector2 linePoint1 = new Vector2(-100, line(-100));
            Vector2 linePoint2 = new Vector2(100, line(100));

            Debug.Log(AWPhysics2D.LineClosestPoint(linePoint1, linePoint2, point));
        }

        private void RoundTest()
        {

        }
    }
}
