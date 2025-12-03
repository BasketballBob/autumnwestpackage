using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class DropdownTemplateCanvas : MonoBehaviour
    {
        [SerializeField]
        private bool _overrideSorting;
        [SerializeField] [ShowIf("@_overrideSorting")]
        private int _canvasSortingOrder;

        private Canvas _canvas;

        private void OnEnable()
        {
            if (_canvas == null) _canvas = GetComponent<Canvas>();
            if (_canvas != null)
            {
                _canvas.overrideSorting = _overrideSorting;
                _canvas.sortingOrder = _canvasSortingOrder;
            }
        }
    }
}
