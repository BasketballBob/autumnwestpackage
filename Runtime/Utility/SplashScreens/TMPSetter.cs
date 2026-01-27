using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AWP
{
    public class TMPSetter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmp;

        private void Reset()
        {
            _tmp = GetComponent<TMP_Text>();
        }

        public void SetText(string text)
        {
            _tmp.text = text;
        }
    }
}
