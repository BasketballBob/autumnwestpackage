using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AWP
{
    public class CreditsText : CreditsObject
    {
        [SerializeField]
        private TMP_Text _text;

        protected override void Reset()
        {
            base.Reset();
            _text = GetComponent<TMP_Text>();
        }

        public TMP_Text Text => _text;
    }
}
