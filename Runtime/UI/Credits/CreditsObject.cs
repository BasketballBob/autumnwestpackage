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

        public TMP_Text Text => _text;
    }
}
