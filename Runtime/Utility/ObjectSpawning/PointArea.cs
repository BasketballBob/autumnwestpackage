using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public struct PointArea
    {
        public AreaType Area;
        public Vector2 Position;
        public Vector2 Size;

        public enum AreaType { Point, Rect }

        public Vector2 Extents => Size / 2;

        public Vector2 GetLocalPoint()
        {
            switch (Area)
            {
                case AreaType.Point:
                    return Position;
                case AreaType.Rect:
                    return (Position - Extents) + new Vector2(Size.x * AWRandom.Range01(), Size.y * AWRandom.Range01());
            }

            throw new NotImplementedException();
        }

        public void DrawGizmos(Vector2 worldPoint)
        {
            Gizmos.color = Color.yellow;

            switch (Area)
            {
                case AreaType.Point:
                    Gizmos.DrawSphere(worldPoint + Position, .1f);
                    break;
                case AreaType.Rect:
                    Gizmos.DrawWireCube(worldPoint + Position, Size);
                    break;
            }
        }
    }
}
