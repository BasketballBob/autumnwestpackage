using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWGizmos
    {
        public static void DrawRect(Rect rect)
        {
            Gizmos.DrawWireCube(rect.center, rect.size);
        }
    }
}
