using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    public static class PolygonCollider2DExtensions
    {
        /// <summary>
        /// Gets a local rect that encompasses all of the polygon colliders points
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Rect GetShapeRect(this PolygonCollider2D col, int shapeIndex = 0)
        {
            Rect rect = new Rect();

            col.points.ForEach(x =>
            {   
                //Debug.Log($"POINT {x}");

                if (x.x > rect.xMax) rect.xMax = x.x;
                if (x.x < rect.xMin) rect.xMin = x.x;
                if (x.y > rect.yMax) rect.yMax = x.y;
                if (x.y < rect.yMin) rect.yMin = x.y;
            });

            //Debug.Log($"RECT xMax:{rect.xMax} xMin{rect.xMin} yMax{rect.yMax} yMin{rect.yMin}");
            return rect;
        }
    }
}
