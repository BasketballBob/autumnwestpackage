using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AWP
{
    public abstract class CreditsObject : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rect;

        public CreditsEntry CreditsEntry { get; set; }

        protected virtual void Reset()
        {
            _rect = GetComponent<RectTransform>();
        }

        public RectTransform Rect => _rect;
        public virtual float Height => _rect.rect.height;
    }
}
