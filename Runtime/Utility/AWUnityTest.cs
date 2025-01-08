using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using AWPEditor;
using Sirenix.Serialization;

namespace AWP
{
    public class AWUnityTest : SerializedMonoBehaviour
    {
        // This class is designated for running test code in Unity

        [SerializeField] [ClampedAnimationCurve]
        private AnimationCurve testCurve;
        [OdinSerialize]
        private SerializedAction testFunc2 = new SerializedAction();
        [SerializeField]
        private CameraPosRef _camPosRef;
        [SerializeField]
        private AWEvent _event;
        [SerializeField] [TypeFilter(nameof(GetTypes))]
        private ValueObject _valueObject;

        [SerializeField]
        private TestCurve _test;

        public IEnumerable<Type> GetTypes() => ValueObject.GetTypes();

        [Button("Test")]
        private void Start()
        {
            //testCurve.DebugDraw();
        }

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
    }
}
