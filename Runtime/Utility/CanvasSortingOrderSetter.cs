using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    public class CanvasSortingOrderSetter : MonoBehaviour
    {
        [Header("MAKE SURE CANVAS IS SET TO ALLOW SORTING TO BE OVERRIDEN (DEBUG INSPECTOR)")]
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private int _sortingOrder;

        private void Start()
        {
            _canvas.sortingOrder = _sortingOrder;
        }
    }
}
