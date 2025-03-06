using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    [System.Serializable] [InlineProperty] [HideLabel]
    public class ItemSelector<TItem> : IEnumerable<TItem>
    {
        [OnValueChanged("OnSelectionTypeChange")] [HideLabel]
        public SelectorType SelectionType;
        [SerializeField] [ShowIf("@IsOfType(SelectorType.Single)")] [HideLabel]
        private TItem _single;
        [SerializeField] [ShowIf("@IsOfType(SelectorType.List)")] [HideLabel]
        private List<TItem> _list;
        [SerializeField] [ShowIf("@IsOfType(SelectorType.WeightedPool)")] [HideLabel]
        private WeightedItemPool<TItem> _weightedPool;

        public enum SelectorType { Single, List, WeightedPool };

        public TItem GetItem()
        {
            switch (SelectionType)
            {
                case SelectorType.Single:
                    return _single;
                case SelectorType.List:
                    return _list[Random.Range(0, _list.Count)];
                case SelectorType.WeightedPool:
                    return _weightedPool.PullItem();
            }

            throw new System.NotImplementedException();
        }

        public IEnumerable<TItem> GetEnumerable()
        {
            switch (SelectionType)
            {
                case SelectorType.Single:
                    return new TItem[] { _single };
                case SelectorType.List:
                    return _list;
                case SelectorType.WeightedPool:
                    return _weightedPool.GetEnumerable();
            }
        
            throw new System.Exception();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #if UNITY_EDITOR
            private void OnSelectionTypeChange()
            {
                //if (SelectionType != SelectorType.Single) _single = default;
                //if (SelectionType != SelectorType.List) _list = null;
                //if (SelectionType != SelectorType.WeightedPool) _weightedPool = null;
            }

            private bool IsOfType(SelectorType selectorType)
            {
                return SelectionType == selectorType;
            }
        #endif
    }
}
