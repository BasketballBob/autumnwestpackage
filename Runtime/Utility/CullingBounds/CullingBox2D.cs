using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class CullingBox2D : CullingBounds
    {
        [SerializeField]
        private Vector2 _boxSize = new Vector2(5, 5);
        [SerializeField]
        private bool _worldSpace;
        [SerializeField] [ShowIf("_worldSpace")]
        private Vector2 _position;

        private Rect _rect;

        public Vector2 Center => _worldSpace ? _position : transform.position;
        public Vector2 Extents => _boxSize / 2;
        public Rect Rect => _rect;

    
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Center, _boxSize);
        }

        protected override void InitializeVariables()
        {
            _rect = new Rect();
            _rect.size = _boxSize;
            _rect.center = Center;
        }

        protected override bool ShouldCull(CullingObject cullingObject)
        {
            if (!Rect.Contains(cullingObject.transform.position))
            {
                return true;
            }

            return false;
        }
    }
}
