using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class LabeledList<TData>
    {
        [SerializeField]
        private List<LabeledItem> _items = new List<LabeledItem>();

        public TData GetItem(string label)
        {
            return GetLabeledItem(label).Value;
        }

        private LabeledItem GetLabeledItem(string label)
        {
            foreach (LabeledItem item in _items)
            {
                if (item.Label == label) return item;
            }

            return default;
        }

        [System.Serializable]
        private struct LabeledItem
        {
            public string Label;
            public TData Value;
        }
    }
}
