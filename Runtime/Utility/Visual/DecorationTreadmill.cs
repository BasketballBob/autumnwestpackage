using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AWP
{
    public abstract class DecorationTreadmill<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _length = 10;
        [SerializeField]
        private ItemSelector<Decoration> _items;

        private List<Decoration> _decorationItems;
        private float _currentLength;
        private float _offset;

        private void Update()
        {
            UpdateDecorations(0);
        }

        private void OnDrawGizmos()
        {
            
        }

        private void UpdateDecorations(float shiftDistance)
        {
            //float currentPos = _offset;
            float currentPos = _offset + shiftDistance;

            for (int i = 0; i < _decorationItems.Count; i++)
            {
                // // Cull items below length
                // if (currentPos < -_decorationItems[i].Extent)
                // {
                //     CullItem();
                // }
                // // Cull items above length
                // else if (currentPos - _length > _decorationItems[i].Extent)
                // {
                //     CullItem();
                // }

                // void CullItem()
                // {
                //     _decorationItems.RemoveAt(i);
                //     i--;
                // }

                //_totalLength += _decorationItems[i].Length; THIS THIS THIS THIS THIS THIS THIS THIS THIS THIS THIS THIS
            }

            while (_currentLength < _length)
            {
                AddItem();
            }
        }

        private void AddItem()
        {
            Decoration decor = _items.GetItem();
            _currentLength += decor.Length;
            _decorationItems.Add(decor);
        }

        protected class Decoration
        {
            public TComponent Component;
            public float Length;

            public Decoration (TComponent component)
            {
                Component = component;
            }

            public float Extent => Length / 2;
        }
    }
}
