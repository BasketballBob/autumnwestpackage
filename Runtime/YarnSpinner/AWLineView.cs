using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;

namespace AWP
{
    public class AWLineView : AWDialogueViewBase
    {
        [SerializeField]
        private TMP_Text _text;

        [Header("Text Printing")]
        [SerializeField]
        private bool _useTypewriterEffect = true;

        public override void DialogueStarted()
        {
            
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StartCoroutine(RunLineRoutine());
            
            IEnumerator RunLineRoutine()
            {
                if (_useTypewriterEffect) yield return PrintText(_text, dialogueLine.TextWithoutCharacterName.Text);

                onDialogueLineFinished?.Invoke();
            }
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StopAnimationRoutine();
            InstantPrintText(_text, dialogueLine.TextWithoutCharacterName.Text);
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

        public override void UserRequestedViewAdvancement()
        {
            requestInterrupt?.Invoke();
        }
    }
}
