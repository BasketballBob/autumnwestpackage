using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Sets object inactive if not all conditions have been met
    /// </summary>
    public class ConditionalActive : MonoBehaviour
    {
        [SerializeField]
        private List<SRWrapper<AWCondition>> _conditions;

        private void Start()
        {
            if (!_conditions.All(x => x.Value.Evaluate()))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
