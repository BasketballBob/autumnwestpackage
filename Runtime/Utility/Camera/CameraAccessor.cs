using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class CameraAccessor : ComponentAccessor<AWCamera>
    {
        protected override AWCamera Component => AWGameManager.AWCamera;

        //public void MoveToCamPos(CameraPos2D camPos) => Component.MoveToCamPos(camPos);
    }
}
