using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace AWP
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _contentArea;
        [SerializeField]
        private ComponentPool<CreditsObject> _headerPool;
        [SerializeField]
        private ComponentPool<CreditsObject> _bodyPool;
        [SerializeField]
        private ComponentPool<CreditsObject> _imagePool;
        [SerializeField]
        private InputActionReference _scrollInput;
        [SerializeField]
        private InputActionReference _speedUpInput;
        [SerializeField]
        private float _scrollSpeed = 200;
        [SerializeField]
        private CreditsData _creditsData;
        
        [Header("Additional settings")]
        [SerializeField]
        private bool _playerScrollingEnabled = true;
        [SerializeField] [ShowIf("@_playerScrollingEnabled")]
        private float _playerScrollSpeed = 100;
        [SerializeField]
        private bool _speedUpEnabled = true;
        [SerializeField] [ShowIf("@_speedUpEnabled")]
        private float _speedUpMultiplier;

        [Header("Events")]
        [SerializeField]
        private bool _loadSceneOnFinish = true;
        [ShowIf("@_loadSceneOnFinish")]
        [SerializeField]
        private string _finishScene;
        [SerializeField]
        private UnityEvent _onFinish;

        [Header("Debug")]
        [SerializeField]
        private bool _debugEnabled;

        [ShowInInspector]
        private List<CreditsEntry> _entryList = new List<CreditsEntry>();
        private GenericRecycler<CreditsEntry> _recycler;
        private SingleCoroutine _scrollRoutine;

        public CreditsData CreditsData
        {
            get => _creditsData;
            set => _creditsData = value;
        }
        private float SpeedUpMultiplier
        {
            get
            {
                if (_speedUpEnabled) return _speedUpMultiplier;
                return 1;
            }
        }

        private void OnEnable()
        {
            if (_scrollInput != null)
            {
                _scrollInput.action.Enable();
                _scrollInput.action.performed += TryScroll;
            }

            if (_speedUpInput != null) _speedUpInput.action.Enable();
        }

        private void OnDisable()
        {
            if (_scrollInput != null)
            {
                _scrollInput.action.Disable();
                _scrollInput.action.performed -= TryScroll;
            }

            if (_speedUpInput != null) _speedUpInput.action.Disable();
        }

        private void Start()
        {
            _recycler = new GenericRecycler<CreditsEntry>(_contentArea.rect.height, SetEnabled, PositionObject);

            _headerPool.Initialize(this, _contentArea);
            _bodyPool.Initialize(this, _contentArea);
            _imagePool.Initialize(this, _contentArea);

            _scrollRoutine = new SingleCoroutine(this);

            Play();
        }

        private void Update()
        {
            
        }

        public void Play()
        {
            InitializeEntryList();
            _scrollRoutine.StartRoutine(PlayRoutine());
        }

        public IEnumerator PlayRoutine()
        {
            yield return _recycler.ScrollRoutine(-_scrollSpeed);
            End();
            
        }

        public void End()
        {
            _scrollRoutine.StopRoutine();

            _onFinish?.Invoke();
            if (_loadSceneOnFinish) AWGameManager.TransitionScene(_finishScene);
        }

        public void Scroll(float offset)
        {
            _recycler.Scroll(offset * _scrollSpeed * Time.deltaTime);
        }

        private void TryScroll(InputAction.CallbackContext context)
        {
            if (!_playerScrollingEnabled) return;
            Scroll(_scrollInput.action.ReadValue<float>());
        }

        private void InitializeEntryList()
        {
            // Generate entries
            _entryList.Clear();
            _creditsData.CreditsItems.ForEach(x =>
            {
                _entryList = _entryList.Concat(x.GetCreditEntries()).ToList();
            });

            _entryList.ForEach(x => x.Pool = GetPool(x));
            _entryList.ForEach(x => x.Size = x.Pool.Prefab.Height);

            InitializeRecycler(_entryList);
        }

        private ComponentPool<CreditsObject> GetPool(CreditsEntry entry)
        {
            if (entry is HeaderObject)
            {
                return _headerPool;
            }
            else if (entry is BodyObject)
            {
                return _bodyPool;
            }
            else if (entry is ImageObject)
            {
                return _imagePool;
            }

            return null;
        }

        #region Generic Recycler
        private void InitializeRecycler(List<CreditsEntry> entries)
        {
            //entries.ForEach(x => )

            for (int i = 0; i < entries.Count; i++)
            {
                _recycler.AddItem(entries[i], entries[i].Size);
            }

            _recycler.SyncActiveItems();
        }

        private void SetEnabled(CreditsEntry entry, bool enabled)
        {
            if (enabled)
            {
                entry.EnsureInstance();

                if (entry is HeaderObject)
                {
                    (entry.Instance as CreditsText).Text.text = (entry as HeaderObject).Text;
                }
                else if (entry is BodyObject)
                {
                    (entry.Instance as CreditsText).Text.text = (entry as BodyObject).Text;
                }
                else if (entry is ImageObject)
                {
                    CreditsImage creditsImage = entry.Instance as CreditsImage;
                    ImageObject imageObject = entry as ImageObject;

                    creditsImage.Image.sprite = imageObject.Image;
                    creditsImage.Rect.sizeDelta = creditsImage.Rect.sizeDelta.SetY(imageObject.Size);
                }
            }
            else entry.DisposeInstance();
        }

        private void PositionObject(CreditsEntry entry, float position)
        {
            entry.Instance.transform.localPosition = new Vector2(_contentArea.rect.center.x,
                _contentArea.rect.max.y - position);
        }
        #endregion
    } 
}
