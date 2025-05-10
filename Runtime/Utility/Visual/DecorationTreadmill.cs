using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace AWP
{
    public abstract class DecorationTreadmill<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _speedMultiplier = 1;
        [SerializeField]
        private float _length = 10;
        [SerializeField]
        private ItemSelector<Decoration> _items;

        [Header("Initial variation")]
        [SerializeField]
        private Vector3 _positionOffsetRange;

        [Header("Delta Animation")]
        [SerializeField]
        protected bool _useDeltaAnimation;
        
        [Header("Scale")]
        [SerializeField] [ShowIf("_useDeltaAnimation")]
        private AnimationCurve _xScaleCurve = AnimationCurve.Constant(0, 1, 1);
        [SerializeField] [ShowIf("_useDeltaAnimation")]
        private AnimationCurve _yScaleCurve = AnimationCurve.Constant(0, 1, 1);
        [SerializeField] [ShowIf("_useDeltaAnimation")]
        private AnimationCurve _zScaleCurve = AnimationCurve.Constant(0, 1, 1);

        [Header("Position")]
        [SerializeField] [ShowIf("_useDeltaAnimation")]
        private AnimationCurve _yPosCurve = AnimationCurve.Constant(0, 1, 1);

        private List<Decoration> _decorationItems = new List<Decoration>();
        private float _currentLength;
        private float _offset;

        public float Speed 
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            UpdateDecorations(_speed * _speedMultiplier * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, 0, _length / 2), 
                new Vector3(Mathf.Max(1, _positionOffsetRange.x * 2), 
                Mathf.Max(1, _positionOffsetRange.y * 2), _length));
        }

        private void Initialize()
        {
            while (_currentLength < _length)
            {
                AddItem(false);
            }

            _offset = 0;
        }

        private void UpdateDecorations(float shiftDistance)
        {
            CheckToCullDecorations(shiftDistance);
            SyncDecorationStates(shiftDistance);

            _offset += shiftDistance;
        }

        private void CheckToCullDecorations(float shiftDistance)
        {
            float currentPos = _offset + shiftDistance;
            bool shiftingBackwards = shiftDistance < 0;

            for (int i = 0; i < _decorationItems.Count; i++)
            {
                if (ItemShouldBeCulled())
                {
                    RemoveItem(_decorationItems[i], shiftingBackwards);
                }
                else currentPos += _decorationItems[i].Length;

                bool ItemShouldBeCulled()
                {
                    if (currentPos + _decorationItems[i].Extent < 0 && shiftingBackwards) 
                    {
                        currentPos += _decorationItems[i].Length;
                        return true;
                    }
                    if (currentPos - _decorationItems[i].Extent > _length && !shiftingBackwards) 
                    {
                        return true;
                    }

                    return false;
                }
            }

            while (_currentLength < _length)
            {
                AddItem(shiftingBackwards);
            }
        }

        private void SyncDecorationStates(float shiftDistance)
        {
            float currentPos = _offset + shiftDistance;

            for (int i = 0; i < _decorationItems.Count; i++)
            {
                _decorationItems[i].SetLocalPosition(CurrentToLocalPos(currentPos));
                CheckApplyDeltaAnimation(_decorationItems[i], Mathf.Clamp01(currentPos / _length));
                currentPos += _decorationItems[i].Length;
            }
        }

        private void AddItem(bool shiftingBackwards)
        {
            Decoration pulledData = _items.GetItem();
            Decoration decor = new Decoration();
            decor.Component = pulledData.Component != null ? Instantiate(pulledData.Component, transform) : null;
            decor.Length = pulledData.Length;
            InitializeDecor(decor);

            decor.SetLocalPosition(CurrentToLocalPos(_currentLength));
            _currentLength += decor.Length;
            if (shiftingBackwards) 
            {
                _decorationItems.Add(decor);
            }
            else 
            {
                _decorationItems.Insert(0, decor);
                _offset -= decor.Length;
            }
        }

        private void RemoveItem(Decoration decor, bool shiftingBackwards)
        {
            _currentLength -= decor.Length;
            _decorationItems.Remove(decor);
            DestroyItem(decor);

            if (shiftingBackwards) 
            {
                _offset += decor.Length;
            }
        }

        protected virtual void DestroyItem(Decoration decor)
        {
            if (decor.Component == null) return;
            Destroy(decor.Component.gameObject);
        }

        private Vector3 CurrentToLocalPos(float currentPos)
        {
            return new Vector3(0, 0, 1) * currentPos;
        }

        #region Initial variation
            private void InitializeDecor(Decoration decor)
            {
                decor.Offset = new Vector3(_positionOffsetRange.x * AWRandom.RangeSigned1(), 
                    _positionOffsetRange.y * AWRandom.RangeSigned1(), 
                    _positionOffsetRange.z * AWRandom.RangeSigned1());
            }
        #endregion

        #region Delta animation
            private void CheckApplyDeltaAnimation(Decoration decor, float delta)
            {
                if (!_useDeltaAnimation) return;
                if (decor.Component == null) return;

                ApplyDeltaAnimation(decor, delta);
            }

            protected virtual void ApplyDeltaAnimation(Decoration decor, float delta)
            {
                decor.Component.transform.localScale = new Vector3(_xScaleCurve.Evaluate(delta),
                    _yScaleCurve.Evaluate(delta), _zScaleCurve.Evaluate(delta));
                
                decor.Component.transform.position += new Vector3(0, _yPosCurve.Evaluate(delta), 0);
            }
        #endregion

        [System.Serializable]
        protected class Decoration
        {
            public TComponent Component;
            public float Length;
            [HideInInspector]
            public Vector3 Offset;

            public Decoration () { }

            public float Extent => Length / 2;

            public void SetLocalPosition(Vector3 localPos)
            {
                if (Component == null) return;
                Component.transform.localPosition = localPos + Offset;
            }
        }
    }
}
