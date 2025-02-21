using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace AWP
{
    #if UNITY_EDITOR
        public static class AWGUILayout
        {
            public static Vector3 Vector3AxisField(Vector3 vector, int axisCount, GUIContent label)
            {
                switch (axisCount)
                {
                    case 1:
                        return new Vector3(EditorGUILayout.FloatField(label, vector.x), 0);
                    case 2:
                        return EditorGUILayout.Vector2Field(label, vector);
                    default:
                        return EditorGUILayout.Vector3Field(label, vector);
                }

                throw new NotImplementedException();
            }

            public static Vector3 Vector3AxisField(Vector3 vector, DimensionType dimensionType, GUIContent label)
            {
                switch (dimensionType)
                {
                    case DimensionType.XY:
                        return Vector3AxisField(vector, 2, label);
                    case DimensionType.XYZ:
                        return Vector3AxisField(vector, 3, label);
                    case DimensionType.XZ:
                        return Vector3XZField(vector, label);
                }

                throw new NotImplementedException();
            }

            public static Vector3 Vector3XZField(Vector3 vector, GUIContent label)
            {
                Vector2 xzVector = new Vector2(vector.x, vector.z);
                xzVector = EditorGUILayout.Vector2Field(label, xzVector);
                return new Vector3(xzVector.x, 0, xzVector.y);
            }
        }
    #endif
}
