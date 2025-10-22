using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
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
        private InputActionReference _scrollInput;
        [SerializeField]
        private float _scrollSpeed = 200;
        [SerializeField]
        private CreditsData _creditsData;

        private float _scrollOffset;
        private int _objectStartIndex;
        private float _contentSize;
        [ShowInInspector]
        private List<CreditsEntry> _entryList = new List<CreditsEntry>();
        private List<CreditsObject> _objectStack = new List<CreditsObject>();

        private void OnEnable()
        {
            _scrollInput.action.Enable();
            _scrollInput.action.performed += DebugScroll;
        }

        private void OnDisable()
        {
            _scrollInput.action.Disable();
            _scrollInput.action.performed -= DebugScroll;
        }

        private void Start()
        {
            _headerPool.Initialize(this);
            _bodyPool.Initialize(this);

            Play();
        }

        private void Update()
        {
            
        }

        public void Play()
        {
            InitializeEntryList();
        }

        public void Scroll(float offset)
        {
            _scrollOffset += offset * _scrollSpeed * Time.deltaTime;

            // Remove top
            while (ItemOutOfBounds(_objectStack.FirstOrDefault()) && _objectStartIndex > 0)
            {
                _objectStartIndex--;
                DisposeItem(_objectStack.PopFirstItem());
            }

            // Remove bottom
            while (ItemOutOfBounds(_objectStack.LastOrDefault()))
            {
                DisposeItem(_objectStack.PopLastItem());
            }

            SyncObjectPositions();
            FillRemainingRoom(offset < 0);
            //SyncObjectPositions();
        }

        private void DebugScroll(InputAction.CallbackContext context)
        {
            if (!AWGameManager.IsDeveloperMode()) return;
            Scroll(_scrollInput.action.ReadValue<float>());
        }
        
        private void FillRemainingRoom(bool fromTop)
        {
            while (CanCreateItem())
            {
                int entryIndex = _objectStartIndex + _objectStack.Count; //fromTop ? _objectStartIndex;
                //_objectStack.Count;

                Debug.Log($"CHECK {entryIndex}");

                if (entryIndex < 0 || entryIndex >= _entryList.Count) break;

                Debug.Log("CREATE OBJ");
                CreditsObject newObj = CreateItem(_entryList[entryIndex]);
                if (fromTop)
                {
                    _objectStack.Insert(0, newObj);
                    _objectStartIndex++;
                }
                else _objectStack.Add(newObj);
            }
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
            _entryList.ForEach(x => x.Height = x.Pool.Prefab.Height);

            float pos = 0;
            for (int i = 0; i < _entryList.Count; i++)
            {
                _entryList[i].Position = pos;
                pos += _entryList[i].Height;
            }

            // // Create as many items as possible
            // for (int i = 0; i < _entryList.Count; i++)
            // {
            //     if (!CanCreateItem()) break;
            //     _objectStack.Add(CreateItem(_entryList[i]));
            // }

            FillRemainingRoom(true);
            SyncObjectPositions();
        }

        private void SyncObjectPositions()
        {
            for (int i = 0; i < _objectStack.Count; i++)
            {
                _objectStack[i].transform.localPosition = new Vector2(0, _objectStack[i].CreditsEntry.Position + _scrollOffset);
            }
        }

        private CreditsObject CreateItem(CreditsEntry entry)
        {
            if (entry is HeaderObject)
            {
                return Create(_headerPool, x => x.Text.text = (entry as HeaderObject).Text);
            }
            else if (entry is BodyObject)
            {
                return Create(_bodyPool, x => x.Text.text = (entry as BodyObject).Text);
            }

            Debug.LogWarning("CREDITS ENTRY LACKS PREFAB COUNTERPART");
            return null;

            CreditsObject Create<TComponent>(ComponentPool<TComponent> pool, Action<TComponent> action) where TComponent : CreditsObject
            {
                TComponent newObject = pool.PullObject();
                newObject.CreditsEntry = entry;
                action(newObject);
                _contentSize += newObject.Height;

                newObject.transform.SetParent(_contentArea);

                return newObject;
            }
        }

        private void DisposeItem(CreditsObject creditsObj)
        {
            _contentSize -= creditsObj.Height;
            creditsObj.CreditsEntry.Pool.DisposeObject(creditsObj);
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

            return null;
        }

        private bool CanCreateItem()
        {
            Debug.Log($"CAN CREATE {_contentSize} < {_contentArea.sizeDelta.y}");
            return _contentSize < _contentArea.sizeDelta.y;
        }
        
        private bool ItemOutOfBounds(CreditsObject obj)
        {
            if (obj == null) return false;
            if (obj.transform.position.y - obj.Height / 2 > _contentArea.rect.max.y) return true;
            if (obj.transform.position.y + obj.Height / 2 < _contentArea.rect.min.y) return true;

            return false;
        }
    } 
}
