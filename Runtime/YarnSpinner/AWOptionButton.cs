using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yarn.Unity;

namespace AWP
{
    [RequireComponent(typeof(Button))]
    public class AWOptionButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmp;

        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
        }

        public void Initialize(DialogueOption dialogueOption, Action onOptionSelected)
        {
            gameObject.SetActive(true);
            _tmp.text = dialogueOption.Line.Text.Text;
            _button.onClick.AddOneShotListener(onOptionSelected);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
