using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class CreditsImage : CreditsObject
    {
        [SerializeField]
        private Image _image;

        protected override void Reset()
        {
            base.Reset();
            _image = GetComponent<Image>();
        }

        public Image Image => _image;
        public override float Height => base.Height;
    }
}
