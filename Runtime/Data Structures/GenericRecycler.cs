using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    [System.Serializable]
    public class GenericRecycler<TData>
    {
        private float _areaSize;
        private float _contentSize;
        private Action<TData, bool> _enableAction;
        private Action<TData, float> _positionAction; 
        private List<Item> _items = new List<Item>();
        private float _offset;

        public float Offset => _offset;
        public float MinOffset => -_contentSize - _items.Last().Size;
        public float MaxOffset => 0;
        public bool AtStartPos => _offset == MaxOffset;
        public bool AtEndPos => _offset == MinOffset;

        public GenericRecycler(float areaSize, Action<TData, bool> enableAction, Action<TData, float> positionAction)
        {
            _areaSize = areaSize;
            _enableAction = enableAction;
            _positionAction = positionAction;
        }

        public void Scroll(float scrollAmount)
        {
            _offset += scrollAmount;
            ClampOffset();

            SyncActiveItems();
        }

        public void SetOffset(float offset)
        {
            _offset = offset;
            ClampOffset();

            SyncActiveItems();
        }

        public void AddItem(TData item, float size)
        {
            _contentSize += size / 2;
            _items.Add(new Item(item, size, _contentSize));
            _contentSize += size / 2;
        }

        public void SyncActiveItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                bool itemEnabled = ItemInBounds(_items[i]);

                _enableAction(_items[i].Data, itemEnabled);
                if (itemEnabled) _positionAction(_items[i].Data, GetCurrentPosition(_items[i]));
            }
        }

        public IEnumerator ScrollRoutine(float scrollSpeed)
        {
            while (!AtEndPos)
            {
                Scroll(scrollSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        private bool ItemInBounds(Item item)
        {
            if (item.Position + item.Size / 2 + _offset < 0) return false;
            if (item.Position - item.Size / 2 + _offset > _areaSize) return false;

            return true;
        }

        private float GetCurrentPosition(Item item)
        {
            return item.Position + _offset;
        }

        private void ClampOffset() => _offset = Mathf.Clamp(_offset, MinOffset, MaxOffset);

        [System.Serializable]
        private class Item
        {
            public TData Data;
            public float Position;
            public float Size;

            public Item(TData data, float size, float position)
            {
                Data = data;
                Size = size;
                Position = position;
            }
        }
    }
}
