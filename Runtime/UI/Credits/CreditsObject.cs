using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AWP
{
    public class CreditsObject : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private RectTransform _rect;

        public CreditsEntry CreditsEntry { get; set; }

        private void Reset()
        {
            _rect = GetComponent<RectTransform>();
        }

        public TMP_Text Text => _text;
        public float Height => _rect.sizeDelta.y;
    }
}
