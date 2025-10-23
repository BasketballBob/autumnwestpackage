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

        public GenericRecycler(float areaSize, Action<TData, bool> enableAction, Action<TData, float> positionAction)
        {
            _areaSize = areaSize;
            _enableAction = enableAction;
            _positionAction = positionAction;
        }

        public void Scroll(float scrollAmount)
        {
            _offset += scrollAmount;
            _offset = Mathf.Clamp(_offset, -_contentSize - _items.Last().Size, 0);

            SyncActiveItems();
        }

        public void AddItem(TData item, float size)
        {
            _contentSize += size;
            _items.Add(new Item(item, size, _contentSize));
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
