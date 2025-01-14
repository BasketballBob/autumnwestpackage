using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AWP
{
    [InlineProperty] [LabelWidth(100)]
    public class AWEvent
    {
        [OdinSerialize]
        private List<SerializedAction> _actions = new List<SerializedAction>();

        public void Invoke()
        {
            foreach (SerializedAction action in _actions)
            {
                action.Invoke();
            }
        }
    }
}
