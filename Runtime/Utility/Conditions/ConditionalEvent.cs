using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    /// <summary>
    /// Runs UnityEvent if all conditions have been met
    /// </summary>
    public class ConditionalEvent : MonoBehaviour
    {
        [SerializeField]
        private List<SRWrapper<AWCondition>> _conditions;
        [SerializeField]
        private UnityEvent _event;

        private void Start()
        {
            if (MeetsConditions()) _event.Invoke();
        }

        public bool MeetsConditions()
        {
            return _conditions.All(x => x.Value.Evaluate());
        }
    }
}
