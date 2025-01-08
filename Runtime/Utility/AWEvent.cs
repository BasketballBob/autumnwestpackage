using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable] [InlineProperty] [LabelWidth(100)]
    public class AWEvent
    {
        [SerializeField] 
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
