using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;

namespace AWP
{
    public class AWDialogueController : AWDialogueViewBase
    {
        private DialogueRunner _dialogueRunner;

        protected virtual bool AdvancePressed => false;
        protected virtual bool InterruptPressed => AdvancePressed;
        protected virtual bool DismissPressed => AdvancePressed;

        private void OnEnable()
        {
            _dialogueRunner = GetComponent<DialogueRunner>();
        }

        private void Update()
        {
            if (AdvancePressed) UserRequestedViewAdvancement();
            if (InterruptPressed) requestInterrupt?.Invoke();
            //if (Dism)
        }

        public override void DialogueStarted()
        {
            
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            onDialogueLineFinished?.Invoke();
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            
        }

        public override void DialogueComplete()
        {
            
        }
    }
}