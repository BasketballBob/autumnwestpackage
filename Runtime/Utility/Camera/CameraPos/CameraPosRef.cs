using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable][InlineProperty]
    public struct CameraPosRef
    {
        [HorizontalGroup("Main")][HideLabel]
        public CameraPosManager Manager;
        [HorizontalGroup("Main")][HideLabel][ValueDropdown("GetPositions")]
        public CameraPos Position;

        public bool PositionedAtSelf => Manager.CurrentPos == Position;

        private IEnumerable GetPositions()
        {
            if (Manager == null) return null;
            return Manager.GetAllPositions();
        }
    }
}
