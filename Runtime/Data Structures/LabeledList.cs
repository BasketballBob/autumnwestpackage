using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public class LabeledList<TData> : IEnumerable<LabeledItem<TData>>
    {
        [SerializeField]
        private List<LabeledItem<TData>> _items = new List<LabeledItem<TData>>();

        public LabeledItem<TData> this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public void Add(string label, TData value)
        {
            _items.Add(new LabeledItem<TData>(label, value));
        }

        public IEnumerator<LabeledItem<TData>> GetEnumerator()
        {
            foreach (LabeledItem<TData> item in _items)
            {
                yield return item;
            }
        }

        public TData GetItem(string label)
        {
            return GetLabeledItem(label).Value;
        }

        public int IndexOf(TData value)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Value.Equals(value))
                {
                    return i;
                }
            }

            return 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private LabeledItem<TData> GetLabeledItem(string label)
        {
            foreach (LabeledItem<TData> item in _items)
            {
                if (item.Label == label) return item;
            }

            return default;
        }
    }
    
    [System.Serializable]
    public struct LabeledItem<TData>
    {
        public string Label;
        public TData Value;

        public LabeledItem(string label, TData value)
        {
            Label = label;
            Value = value;
        }
    }
}
