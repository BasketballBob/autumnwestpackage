using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class CameraPosCaller : MonoBehaviour
    {
        [SerializeField]
        private CameraPosRef _cameraPosRef;
        [SerializeField]
        private CameraPosRef _ifAlreadyAt;
        [SerializeField]
        private bool _overrideDefaultValues;
        [SerializeField] [ShowIf("_overrideDefaultValues")]
        private float _duration = 1;
        [SerializeField] [ShowIf("_overrideDefaultValues")]
        private EasingFunction _easingMode;

        public void MoveTowards()
        {
            if (_ifAlreadyAt.Position != null && _cameraPosRef.PositionedAtSelf)
            {
                MoveTowards(_ifAlreadyAt);
            }
            else MoveTowards(_cameraPosRef);
        }

        public void MoveTowards(CameraPosRef camPosRef)
        {
            if (_overrideDefaultValues)
            {
                camPosRef.Manager.MoveToCamPos(camPosRef.Position, _duration, _easingMode);
            }
            else camPosRef.Manager.MoveToCamPos(camPosRef.Position);
        }

        public void SetCameraPosRef(CameraPosRef newRef) => _cameraPosRef = newRef;
        public CameraPosRef GetCameraPosRef() => _cameraPosRef;
        public void SetAlreadyAtRef(CameraPosRef newRef) => _ifAlreadyAt = newRef; 
        public CameraPosRef GetAlreadyAtRef() => _ifAlreadyAt;
    }
}
