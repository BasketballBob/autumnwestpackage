using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    public class CameraPosManager : MonoBehaviour
    {
        [SerializeReference]
        private IAWCameraReference _cameraRef;
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

        private bool _moving;

        public CameraPos CurrentPos { get; private set; }

        public void MoveToCamPos(CameraPos camPos, float duration, EasingFunction easing, AWDelta.DeltaType deltaType = AWCamera.DefaultDeltaType, Action onFinish = null)
        {
            _moving = true; 

            _cameraRef.Camera.MoveToCamPos(camPos, duration, easing, deltaType, () => 
            {
                onFinish?.Invoke();
                OnMoveFinish?.Invoke(camPos);
                _moving = false;
            });
            CurrentPos = camPos;

            OnMoveStart?.Invoke(CurrentPos);
        }
        private void MoveToCamPos(CameraPosData data)
        {
            MoveToCamPos(data.CameraPos, data.ShiftSettings.Duration, data.ShiftSettings.EasingMode);
        }
        public void MoveToCamPos(CameraPos camPos)
        {
            CameraPosData data = GetCameraPosData(camPos);
            MoveToCamPos(data);
        }
        public void MoveToCamPos(string camPos)
        {
            CameraPosData camPosData = GetCameraPosData(camPos);
            MoveToCamPos(camPosData);
        }

        /// <summary>
        /// Moves to position if it isn't the previously moved to CameraPos
        /// </summary>
        /// <param name="camPos"></param>
        public void TryMoveToCamPos(CameraPos camPos)
        {
            if (camPos == CurrentPos) return;
            MoveToCamPos(camPos);
        }
        public void TryMoveToCamPos(string camPos) => TryMoveToCamPos(GetCameraPosData(camPos).CameraPos);

        public IEnumerator WaitForMoveToFinish()
        {
            while (_moving)
            {
                yield return null;
            }
        }

        public bool IsCurrentPos(string camPos)
        {
            return CurrentPos == GetCameraPosData(camPos).CameraPos;
        }

        public IEnumerable GetAllPositions()
        {
            if (_positions.IsNullOrEmpty()) return null;
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
