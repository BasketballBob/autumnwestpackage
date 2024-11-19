using System;
using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AWOptionView : AWDialogueViewBase
    {
        [SerializeField]
        private AWOptionButton _optionButtonPrefab;

        private List<AWOptionButton> _optionButtons = new List<AWOptionButton>();

        private void Start()
        {
            _optionButtons.Add(_optionButtonPrefab);
            _optionButtonPrefab.Disable();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            base.RunOptions(dialogueOptions, onOptionSelected);

            SyncButtonCount(dialogueOptions.Length);
            SyncButtonValues(dialogueOptions, onOptionSelected);
        }

        private void SyncButtonCount(int buttonCount)
        {
            // Create buttons if not enough
            while (_optionButtons.Count < buttonCount)
            {
                AWOptionButton option = Instantiate(_optionButtonPrefab.gameObject, 
                    _optionButtonPrefab.transform.parent).GetComponent<AWOptionButton>();
                _optionButtons.Add(option);
            }

            // Disable buttons if too many
            for (int i = _optionButtons.Count - 1; i >= 0; i--)
            {
                if (i > buttonCount)
                {
                    _optionButtons[i].Disable();
                }
            }
        }

        private void SyncButtonValues(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                int optionIndex = i;
                _optionButtons[i].Initialize(dialogueOptions[i], () => 
                {
                    onOptionSelected(optionIndex);
                    DisableButtons();
                });
            }
        }

        private void DisableButtons()
        {
            _optionButtons.ForEach((x) => x.Disable());
        }
    }
}
