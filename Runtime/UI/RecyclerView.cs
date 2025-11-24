using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public abstract class RecyclerView<TComponent, TData> : ObjectPool<RectTransform>
    {
        [SerializeField]
        protected RectTransform _rect;
        [SerializeField]
        private Scrollbar _scrollbar;
        [SerializeField]
        private float _offset;
        [SerializeField] [MinValue(0)]
        private float _upperMargin;
        [SerializeField] [MinValue(0)]
        private float _lowerMargin;

        protected List<TData> _data = new List<TData>();
        private List<TComponent> _components = new List<TComponent>();
        private int _dataIndexOffset;
        private int _oldDataIndexOffset;
        private int _targetActiveCount;
        private int _oldTargetActiveCount;

        public List<TData> Data => _data;
        public float Offset => _offset;
        public float OffsetDelta => _offset.GetDelta(MinOffset, MaxOffset);
        public float RectLength => _rect.sizeDelta.y;
        public float MinOffset => -_upperMargin;
        public float MaxOffset => Mathf.Max(DataTotalLength - RectLength, 0);
        public float PrefabLength => _prefab.sizeDelta.y;
        public float DataTotalLength => PrefabLength * _data.Count + _upperMargin + _lowerMargin;

        /// <summary>
        /// The length of the currently displayed items
        /// </summary>
        public float DisplayedLength { get; private set; }
        public float DisplayedMaxY => _rect.rect.max.y - (_dataIndexOffset < 0 ? PrefabLength * Mathf.Abs(_dataIndexOffset) : 0);
        public float DisplayedMinY => DisplayedMaxY - DisplayedLength;
        protected virtual bool ModifyScrollbarSize => true;


        protected virtual void OnEnable()
        {
            if (_scrollbar != null) _scrollbar.onValueChanged.AddListener(SyncScrollbar);
        }

        protected virtual void OnDisable()
        {
            if (_scrollbar != null) _scrollbar.onValueChanged.RemoveListener(SyncScrollbar);
        }

        protected override void Start()
        {
            base.Start();

            _offset = MinOffset;
            SyncView();
        }

        private void LateUpdate()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.yellow;
            if (_rect != null && (_upperMargin > 0 || _lowerMargin > 0))
            {
                Rect drawRect = _rect.rect;
                drawRect.yMax -= _upperMargin;
                drawRect.yMin += _lowerMargin;

                Gizmos.DrawWireCube(drawRect.center, drawRect.size);
            }
        }

        private void Reset()
        {
            _rect = GetComponent<RectTransform>();
        }

        protected sealed override void AddObjectToPool(RectTransform obj)
        {
            base.AddObjectToPool(obj);
            _components.Add(obj.GetComponent<TComponent>());
            InitializeComponent(_components.Last());
        }

        protected sealed override bool ObjectIsActive(RectTransform obj)
        {
            return obj.gameObject.activeSelf;
        }

        protected sealed override void SyncObjectValues(RectTransform obj, int index)
        {
            base.SyncObjectValues(obj, index);

            int dataIndex = index;
            if (_dataIndexOffset >= 0) dataIndex += _dataIndexOffset;

            SyncObjectValues(_components[index], _data[dataIndex]);
        }

        private void SyncObjectPositions()
        {
            ModifyActiveObjects((x, index) =>
            {
                float position = PrefabLength * index + (-_offset % PrefabLength) + PrefabLength / 2;
                if (_dataIndexOffset < 0) position += PrefabLength * Mathf.Abs(_dataIndexOffset);

                SetPosition(position, index);
            });
        }

        protected abstract void InitializeComponent(TComponent component);
        protected abstract void SyncObjectValues(TComponent component, TData data);

        [Button]
        protected virtual void SyncView()
        {
            _offset += PrefabLength * (_dataIndexOffset - _oldDataIndexOffset);
            _offset = Mathf.Clamp(_offset, MinOffset, MaxOffset);
            _dataIndexOffset = (int)(_offset / PrefabLength);
            _targetActiveCount = GetTargetActiveCount();

            // Scrollbar
            if (_scrollbar != null && ModifyScrollbarSize)
            {
                if (DataTotalLength > 0) _scrollbar.size = 1 - (MaxOffset / DataTotalLength);
                else _scrollbar.size = 1;

                _scrollbar.gameObject.SetActive(_scrollbar.size != 1);
            }

            SetActiveCount(_targetActiveCount);

            bool indexOffsetChanged = _dataIndexOffset != _oldDataIndexOffset;
            bool targetCountChanged = _targetActiveCount != _oldTargetActiveCount;

            if (indexOffsetChanged && !targetCountChanged)
            {
                int indexChange = _dataIndexOffset - _oldDataIndexOffset;
                while (indexChange < 0)
                {
                    MoveItemIndex(Items.Count - 1, 0);
                    indexChange++;
                }
                while (indexChange > 0)
                {
                    MoveItemIndex(0, Items.Count - 1);
                    indexChange--;
                }
            }

            SyncObjectPositions();
            if (indexOffsetChanged || targetCountChanged)
            {
                SyncActiveValues();
            }

            _oldDataIndexOffset = _dataIndexOffset;
            _oldTargetActiveCount = _targetActiveCount;
        }

        /// <summary>
        /// Calculates the target amount of active objects needed to fill the display
        /// </summary>
        /// <returns></returns>
        private int GetTargetActiveCount()
        {
            int targetCount = ActiveItemCount;
            float visibleDistance = RectLength + PrefabLength;
            if (_offset < 0) visibleDistance += _offset;

            targetCount = Mathf.CeilToInt(visibleDistance / PrefabLength);

            while (targetCount * PrefabLength < visibleDistance)
            {
                targetCount++;
            }

            if (targetCount > _data.Count) targetCount = _data.Count;
            if (targetCount > _data.Count - _dataIndexOffset && _dataIndexOffset > 0) targetCount = _data.Count - _dataIndexOffset;

            //Debug.Log($"TARGETCOUNT {targetCount}");

            // Sync displayed values
            DisplayedLength = PrefabLength * targetCount;

            return targetCount;
        }

        private void SetPosition(float position, int index)
        {
            Items[index].localPosition = new Vector3(0, -position + _rect.rect.max.y);
        }

        #region Item management
        private void MoveItemIndex(int itemIndex, int moveIndex)
        {
            Items.MoveToIndex(itemIndex, moveIndex);
            _components.MoveToIndex(itemIndex, moveIndex);
        }
        #endregion

        #region Scrollbar
        protected void SetOffset(float value)
        {
            SyncScrollbar(value);
            _scrollbar.value = OffsetDelta;
        }

        private void SyncScrollbar(float value)
        {
            _offset = MinOffset.Lerp(MaxOffset, value);

            SyncView();
        }

        protected void Scroll(float offsetChange)
        {
            if (MaxOffset == 0) return;
            _scrollbar.value += offsetChange / MaxOffset;
            _scrollbar.value = Mathf.Clamp01(_scrollbar.value);
            //_offset += offsetChange;
        }
        #endregion
    }
}