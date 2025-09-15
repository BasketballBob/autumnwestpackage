using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    [RequireComponent(typeof(Button))]
    public class AWButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _label;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetButtonData(AWButtonData data)
        {
            _label.text = data.Label;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => data.SelectAction());
        }
    }

    public class AWButtonData
    {
        public string Label;
        public Action SelectAction;

        public AWButtonData(string label, Action onSelect)
        {
            Label = label;
            SelectAction = onSelect;
        }
    }
}
