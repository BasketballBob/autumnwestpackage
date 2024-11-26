using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class PlotFunctionData
    {
        public Vector2 LocalPos;
        public Vector2 MoveStep;

        public PlotFunctionData() { }

        public PlotFunctionData(Vector2 localPos, Vector2 moveStep)
        {
            LocalPos = localPos;
            MoveStep = moveStep;
        }
    }
}
