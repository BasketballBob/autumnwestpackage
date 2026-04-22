using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Class that pulls the first item that meets given conditions (if any)
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    [System.Serializable]
    public class ConditionalList<TItem, TCondition> where TCondition : AWCondition
    {
        [SerializeField]
        private List<ConditionalItem> _conditionalItems = new List<ConditionalItem>();

        public ConditionalList() { }

        /// <summary>
        /// Gets the first item in the list that passes conditions
        /// </summary>
        /// <returns>Item found</returns>
        public bool GetItem(out TItem item)
        {
            for (int i = 0; i < _conditionalItems.Count; i++)
            {
                if (_conditionalItems[i].PassesConditions())
                {
                    item = _conditionalItems[i].Item;
                    return true;
                }
            }

            item = default;
            return false;
        }
        
        [System.Serializable]
        public class ConditionalItem
        {
            public TItem Item;
            public List<SRWrapper<TCondition>> Conditions = new List<SRWrapper<TCondition>>();

            public bool PassesConditions()
            {
                if (Conditions.IsNullOrEmpty()) return true;
                
                return Conditions.All(x => x.Value.Evaluate());
            }
        }
    }

    //[System.Serializable]
    //public class ConditionalList<TItem> : ConditionalList<TItem, AWCondition> { }
}
