using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AWP
{
    /// <summary>
    /// Used to automatically scroll a ScrollRect to follow selected UI
    /// Adapted from Clown Meat script "ButtonScrollRect.cs"
    /// </summary>
    public class ScrollRectAutoNavigation : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private Vector2 _clampArea = new Vector2(100, 100);
        [SerializeField]
        private GameObject _initialSelectedObject;
        [SerializeField]
        private float _moveDuration = .1f;

        private GameObject _oldSelectedObject;
        private SingleCoroutine _moveRoutine;

        private void Reset()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Start()
        {
            _moveRoutine = new SingleCoroutine(this);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.viewport);

            if (_initialSelectedObject != null) MoveInstant(_initialSelectedObject);
        }

        private void LateUpdate()
        {
            CheckIfSelectionHasChanged();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_scrollRect.transform.parent.TransformPoint(_scrollRect.viewport.rect.center), 
                _scrollRect.transform.parent.TransformVector(_scrollRect.viewport.rect.size));

            if (_oldSelectedObject != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(_oldSelectedObject.transform.position, 25);
            }
        }

        #region Selection management
        private void OnSelectionChange(GameObject newSelection)
        {
            if (newSelection == null) return;
            CheckToMove(newSelection);
        }

        private void CheckIfSelectionHasChanged()
        {
            if (EventSystem.current.currentSelectedGameObject != _oldSelectedObject)
            {
                OnSelectionChange(EventSystem.current.currentSelectedGameObject);
                _oldSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
        }
        #endregion

        private void CheckToMove(GameObject newSelection)
        {
            _moveRoutine.StartRoutine(Move(newSelection, _moveDuration));
        }

        /// <summary>
        /// Gets the offset of the selected element and the center of the viewport
        /// Clamped by the the max possible movement of the content within the viewport
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        private Vector2 GetClampedOffset(GameObject selection)
        {
            Rect contentRect = _scrollRect.content.GetWorldRect();
            Rect viewportRect = _scrollRect.viewport.GetWorldRect();
            Vector2 selectedOffset = selection.transform.position - _scrollRect.viewport.position;

            // Clamp by possible offset (max movement of content within viewport)
            Vector2 clampedOffset = new Vector2(Mathf.Clamp(selectedOffset.x, contentRect.min.x - viewportRect.min.x, contentRect.max.x - viewportRect.max.x),
                Mathf.Clamp(selectedOffset.y, contentRect.min.y - viewportRect.min.y, contentRect.max.y - viewportRect.max.y));

            // Zero offset if not moving on axis
            if (!_scrollRect.horizontal) clampedOffset = clampedOffset.SetX(0);
            if (!_scrollRect.vertical) clampedOffset = clampedOffset.SetY(0);

            return clampedOffset;
        }

        private IEnumerator Move(GameObject selection, float duration)
        {
            Vector2 startPos = _scrollRect.content.position;

            yield return AnimationFX.DeltaRoutine(x =>
            {
                _scrollRect.content.position = startPos + (((Vector2)_scrollRect.content.position - GetClampedOffset(selection)) - startPos) * x;
            }, duration, EasingFunction.Sin, AWDelta.DeltaType.UnscaledUpdate);
        }

        private void MoveInstant(GameObject selection)
        {
            _scrollRect.content.position -= (Vector3)GetClampedOffset(selection);
        }
    }
}
