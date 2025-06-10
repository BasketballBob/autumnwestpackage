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
        private RectTransform _rect;
        [SerializeField]
        private Scrollbar _scrollbar;
        [SerializeField]
        private float _offset;

        protected List<TData> _data = new List<TData>();
        private List<TComponent> _components = new List<TComponent>();
        private int _dataIndexOffset;
        private int _oldDataIndexOffset;
        private int _targetActiveCount;
        private int _oldTargetActiveCount;

        public List<TData> Data => _data;
        public float RectLength => _rect.sizeDelta.y;
        public float MaxOffset => Mathf.Max(DataTotalLength - RectLength, 0);
        public float PrefabLength => _prefab.sizeDelta.y;
        public float DataTotalLength => PrefabLength * _data.Count;

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
            SyncView();
        }

        private void LateUpdate()
        {

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

            int dataIndex = index + _dataIndexOffset;
            SyncObjectValues(_components[index], _data[dataIndex]);
        }

        private void SyncObjectPositions()
        {
            ModifyActiveObjects((x, y) =>
            {
                float position = PrefabLength * y + (-_offset % PrefabLength) + PrefabLength / 2;
                SetPosition(position, y);
            });
        }

        protected abstract void InitializeComponent(TComponent component);
        protected abstract void SyncObjectValues(TComponent component, TData data);

        [Button]
        protected void SyncView()
        {
            _offset += PrefabLength * (_dataIndexOffset - _oldDataIndexOffset);
            _offset = Mathf.Clamp(_offset, 0, MaxOffset);
            _dataIndexOffset = Mathf.FloorToInt(_offset / PrefabLength);
            _targetActiveCount = GetTargetActiveCount();

            if (_scrollbar != null)
            {
                if (DataTotalLength > 0) _scrollbar.size = 1 - (MaxOffset / DataTotalLength);
                else _scrollbar.size = 1;

                _scrollbar.gameObject.SetActive(_scrollbar.size != 1);
            }

            SetActiveCount(_targetActiveCount);
            SyncObjectPositions();

            bool indexOffsetChanged = _dataIndexOffset != _oldDataIndexOffset;
            bool targetCountChanged = _targetActiveCount != _oldTargetActiveCount;

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

            targetCount = Mathf.CeilToInt(visibleDistance / PrefabLength);

            while (targetCount * PrefabLength < visibleDistance)
            {
                targetCount++;
            }

            if (targetCount > _data.Count) targetCount = _data.Count;
            if (targetCount > _data.Count - _dataIndexOffset) targetCount = _data.Count - _dataIndexOffset;

            return targetCount;
        }

        private void SetPosition(float position, int index)
        {
            Items[index].localPosition = new Vector3(0, -position + _rect.rect.max.y);
        }

        #region Scrollbar
        private void SyncScrollbar(float value)
        {
            _offset = MaxOffset * value;

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
