using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(Renderer))]
    public class SetSortingOrder : MonoBehaviour
    {
        [SerializeField] [ValueDropdown("LayerOptions")]
        private string _layer;
        [SerializeField]
        private int _sortingOrder;

        private Renderer _renderer;

        private void OnEnable()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            _renderer.sortingLayerName = _layer;
            _renderer.sortingOrder = _sortingOrder;
        }

        private IEnumerable LayerOptions()
        {
            return SortingLayer.layers.Select(x => x.name);
        }
    }
}
