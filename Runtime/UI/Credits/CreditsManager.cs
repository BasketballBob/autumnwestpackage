using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AWP
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _header;
        [SerializeField]
        private TMP_Text _bodyText;
        [SerializeField]
        private ComponentPool<TMP_Text> _headers;
        [SerializeField]
        private ComponentPool<TMP_Text> _bodyTexts;
        [SerializeField]
        private CreditsData _creditsData;

        private List<CreditsObject> _creditsObjects = new List<CreditsObject>();

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            
        }
    }
}
