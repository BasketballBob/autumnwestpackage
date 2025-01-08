using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class CameraPosManager : MonoBehaviour
    {
        [SerializeField]
        private AWCamera _cameraRef;
        [SerializeField]
        private List<CameraPosData> _positions = new List<CameraPosData>();

        public Action<CameraPos> OnMoveStart;
        public Action<CameraPos> OnMoveFinish;

        public CameraPos CurrentPos { get; private set; }

        public void MoveToCamPos(CameraPos camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = AWCamera.DefaultDeltaType, Action onFinish = null)
        {
            _cameraRef.MoveToCamPos(camPos, duration, easing, deltaType, () => 
            {
                onFinish?.Invoke();
                OnMoveFinish(camPos);
            });
            CurrentPos = camPos;

            OnMoveStart.Invoke(CurrentPos);
        }
        public void MoveToCamPos(CameraPos camPos)
        {
            CameraPosData camPosData = GetCameraPosData(camPos);
            MoveToCamPos(camPos, camPosData.ShiftSettings.Duration, camPosData.ShiftSettings.EasingMode);
        }

        public IEnumerable GetAllPositions()
        {
            if (_positions == null) return null;
            return _positions.Select(x => new ValueDropdownItem(x.CameraPos.name, x.CameraPos));
        }

        private CameraPosData GetCameraPosData(CameraPos cameraPos)
        {
            foreach (CameraPosData data in _positions)
            {
                if (cameraPos == data.CameraPos) return data;
            }

            throw new System.Exception("Camera pos data doesn't exist!");
        }

        [System.Serializable]
        private class CameraPosData
        {
            public CameraPos CameraPos;
            public ShiftSettings ShiftSettings;
        }
    }
}
