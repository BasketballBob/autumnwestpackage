using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class PopupMenu : Menu
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private AWButton _buttonBase;

        private List<AWButton> _buttons = new List<AWButton>();

        protected override void Start()
        {
            base.Start();

            _buttons.Add(_buttonBase);
        }

        public void Display(PopupData data)
        {
            _text.text = data.Text;
            SyncButtonCount(data.ButtonData.Count);
            for (int i = 0; i < data.ButtonData.Count; i++)
            {
                _buttons[i].SetButtonData(data.ButtonData[i]);
            }

            PushSelf();
        }

        public void SyncButtonCount(int count)
        {
            // Create new buttons if necessary
            while (_buttons.Count < count)
            {
                AWButton button = Instantiate(_buttonBase, _buttonBase.transform.parent);
                _buttons.Add(button);
            }

            // Set enabled count
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].gameObject.SetActive(i < count);
            }
        }

        public class PopupData
        {
            public string Text;
            public List<AWButtonData> ButtonData = new List<AWButtonData>();
        }
    }
}
