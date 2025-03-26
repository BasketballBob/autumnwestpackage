using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class IBoolEvent : SerializedMonoBehaviour
    {
        [SerializeField]
        private UnityEvent _event;
        [OdinSerialize]
        private ItemSelector<IBool> _bools;

        private void Start()
        {
            if (ShouldRunEvent()) RunEvent();
        }

        private bool ShouldRunEvent()
        {
            foreach (IBool element in _bools)
            {
                if (!element.EvaluateBool())
                {
                    return false;
                }
            }

            return true;
        }

        private void RunEvent()
        {
            _event?.Invoke();
        }
    }
}
