using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AWP
{
    [InlineProperty] [HideReferenceObjectPicker] [LabelWidth(100)]
    public class AWEvent
    {
        [OdinSerialize] [HideReferenceObjectPicker] [InlineProperty] [ListDrawerSettings(CustomAddFunction = "CustomAddFunction", ShowFoldout = false)]
        private List<SerializedAction> _actions = new List<SerializedAction>();

        public void Invoke()
        {
            foreach (SerializedAction action in _actions)
            {
                action.Invoke();
            }
        }

        private SerializedAction CustomAddFunction() => new SerializedAction();
    }
}
