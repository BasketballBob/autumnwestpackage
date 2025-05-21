using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class CameraPosEvents : MonoBehaviour
    {
        [SerializeField]
        private CameraPosManager _manager;
        [SerializeField] [ListDrawerSettings(CustomAddFunction = "AddEvent")]
        private List<CameraPosEvent> _events = new List<CameraPosEvent>();

        private void OnEnable()
        {
            _manager.OnMoveStart += OnMove;
        }

        private void OnDisable()
        {
            _manager.OnMoveStart -= OnMove;
        }

        private void OnMove(CameraPos camPos)
        {
            foreach (CameraPosEvent element in _events)
            {
                if (element.Pos == camPos) element.Event.Invoke();
            }
        }

        private void AddEvent()
        {
            _events.Add(new CameraPosEvent(this));
        }

        [System.Serializable]
        public class CameraPosEvent
        {
            [ValueDropdown("GetPositions")]
            public CameraPos Pos;
            public UnityEvent Event;

            [SerializeField]
            [HideInInspector]
            private CameraPosEvents _parent;

            public CameraPosEvent(CameraPosEvents parent)
            {
                _parent = parent;
            }

            private IEnumerable GetPositions()
            {
                if (_parent == null) return null;
                if (_parent._manager == null) return null;
                return _parent._manager.GetAllPositions();
            }
        }
    }
}
