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

        [Header("Debug")]
        [SerializeField] [ValueDropdown("PositionOptions")]
        private CameraPos _testCamPos;
        [Button("TestPos")]
        private void TestPos()
        {
            if (!Application.isPlaying) return;
            MoveToCamPos(_testCamPos);
        }

        public Action<CameraPos> OnMoveStart;
        public Action<CameraPos> OnMoveFinish;

        public CameraPos CurrentPos { get; private set; }

        public IEnumerator MoveToCamPos(CameraPos camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = AWCamera.DefaultDeltaType, Action onFinish = null)
        {
            bool finished = false;

            _cameraRef.MoveToCamPos(camPos, duration, easing, deltaType, () => 
            {
                onFinish?.Invoke();
                OnMoveFinish?.Invoke(camPos);
                finished = true;
            });
            CurrentPos = camPos;

            OnMoveStart?.Invoke(CurrentPos);

            while (!finished)
            {
                yield return null;
            }
        }
        private IEnumerator MoveToCamPos(CameraPosData data)
        {
            return MoveToCamPos(data.CameraPos, data.ShiftSettings.Duration, data.ShiftSettings.EasingMode);
        }
        public IEnumerator MoveToCamPos(CameraPos camPos)
        {
            CameraPosData data = GetCameraPosData(camPos);
            return MoveToCamPos(data);
        }
        public IEnumerator MoveToCamPos(string camPos)
        {
            CameraPosData camPosData = GetCameraPosData(camPos);
            return MoveToCamPos(camPosData);
        }

        public IEnumerable GetAllPositions()
        {
            if (_positions == null) return null;
            return _positions.Select(x => new ValueDropdownItem(x.CameraPos.name, x.CameraPos));
        }

        private CameraPosData GetCameraPosData(string name)
        {
            for (int i = 0; i < _positions.Count; i++)
            {
                if (_positions[i].CameraPos.name == name)
                {
                    return _positions[i];
                }
            }   

            return null;
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

        #if UNITY_EDITOR
            private IEnumerable PositionOptions()
            {
                return _positions.Select(x => new ValueDropdownItem(x.CameraPos?.name ?? "EMPTY", x.CameraPos));
            }
        #endif
    }
}
