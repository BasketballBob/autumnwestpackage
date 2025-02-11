using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.CheckIn.Progress;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class TreadmillPool : StaticObjectPool<Renderer>
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private TreadmillDirection _direction;

        protected enum TreadmillDirection { X, Y, Z };

        public float OffsetDelta
        {
            get { return _offsetDelta; }
            set 
            { 
                _offsetDelta = value; 
                while (_offsetDelta < 0) _offsetDelta += 1;
                while (_offsetDelta > 1) _offsetDelta -= 1;
            }
        }
        private float _offsetDelta;
        public virtual Vector3 ItemSize => _prefab.bounds.size;

        private void Update()
        {
            ModifyActiveItems();
            OffsetDelta += _speed * Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _minItemCount; i++)
            {
                Gizmos.matrix = transform.localToWorldMatrix;

                Gizmos.color = Color.yellow;
                if (i == 0) Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(GetItemPosition(i, 0), ItemSize);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(GetItemPosition(_minItemCount - 1, 1), ItemSize);
        }

        protected override void ActiveModification(Renderer obj, int index)
        {
            obj.transform.localPosition = GetItemPosition(index, OffsetDelta);
        }

        protected override bool ObjectIsActive(Renderer obj)
        {
            return obj.gameObject.activeSelf;
        }

        protected Vector3 GetItemPosition(int index, float delta)
        {
            switch (_direction)
            {
                case TreadmillDirection.X:
                    break;
                case TreadmillDirection.Y:
                    break;
                case TreadmillDirection.Z:
                    return new Vector3(0, 0, ItemSize.z * index +
                        ItemSize.z * delta);
            }

            throw new NotImplementedException();
        }
    }
}
