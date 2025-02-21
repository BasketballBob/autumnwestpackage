using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#if UNITY_EDITOR
using UnityEditor;
using AWPEditor;
//using AWPEditor.Animatio
#endif

namespace AWP
{
    [System.Serializable]
    public class PointArea
    {
        [OnValueChanged("OnDimensionTypeChange")]
        public DimensionType DimensionType;
        public AreaType Area;
        public Vector3 Position;
        [CustomValueDrawer("SizeValueDrawer")] [HideIf("@Area == AreaType.Point")]
        public Vector3 Size = Vector3.one;

        public enum AreaType { Point, Cube, Sphere }

        public Vector3 Extents => Size / 2;

        public Vector3 GetLocalPoint()
        {
            return DimensionType.ZeroVector3(GetPoint());

            Vector3 GetPoint()
            {
                switch (Area)
                {
                    case AreaType.Point:
                        return Position;
                    case AreaType.Cube:
                        return (Position - Extents) + new Vector3(Size.x * AWRandom.Range01(), Size.y * AWRandom.Range01(), Size.z * AWRandom.Range01());
                    case AreaType.Sphere: 
                        return UnityEngine.Random.insideUnitSphere * Size.x;
                }

                throw new NotImplementedException();
            }
        }
        public Vector3 GetWorldPoint(Transform trans) => trans.TransformPoint(GetLocalPoint());

        public void DrawGizmos(Transform trans)
        {
            Gizmos.matrix = trans.localToWorldMatrix;

            switch (Area)
            {
                case AreaType.Point:
                    Gizmos.DrawSphere(Position, .1f);
                    break;
                case AreaType.Cube:
                    Gizmos.DrawWireCube(Position, DimensionType.ZeroVector3(Size));
                    break;
                case AreaType.Sphere:
                    Gizmos.DrawWireSphere(Position, Size.x);
                    break;
            }
        }

        #if UNITY_EDITOR
            private void OnDimensionTypeChange()
            {
                
            }

            private Vector3 SizeValueDrawer(Vector3 size, GUIContent label)
            {
                if (Area == AreaType.Sphere)
                {
                    return AWGUILayout.Vector3AxisField(size, 1, label);
                }
                else
                {
                    return AWGUILayout.Vector3AxisField(size, DimensionType, label);
                }
                
                throw new NotImplementedException();
            }
        #endif
    }
}
