using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class Camera2DAccessor : ComponentAccessor<Camera2D>
    {
        protected override Camera2D Component => AWGameManager.Camera2D;

        //public void MoveToCamPos(CameraPos2D camPos) => Component.MoveToCamPos(camPos);
    }
}
