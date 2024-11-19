using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;
using Sirenix.Utilities;

namespace AWP
{
    [RequireComponent(typeof(DialogueRunner))]
    public class AWDialogueController : AWDialogueViewBase
    {
        private DialogueRunner _dialogueRunner;

        protected virtual bool AdvancePressed => false;
        protected virtual bool InterruptPressed => AdvancePressed;
        protected virtual bool DismissPressed => AdvancePressed;

        protected override void OnEnable()
        {
            base.OnEnable();

            _dialogueRunner = GetComponent<DialogueRunner>();
        }

        public override void DialogueStarted()
        {
            StartAnimationRoutine(StartDialogueAnimation());
        }   

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            StartCoroutine(RunLineRoutine());

            IEnumerator RunLineRoutine()
            {
                while (AnimationActive) yield return null;
                base.RunLine(dialogueLine, onDialogueLineFinished);
            }
        }

        // public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        // {

        // }

        // public override void DismissLine(Action onDismissalComplete)
        // {
        //     onDismissalComplete?.Invoke();
        // }

        // public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        // {
        //     //onOptionSelected(0);
        // }

        public override void DialogueComplete()
        {
            StartAnimationRoutine(EndDialogueAnimation());
        }

        public override void UserRequestedViewAdvancement()
        {
            _dialogueRunner.dialogueViews.ForEach((x) => 
            {
                if (x == this) return;
                x.UserRequestedViewAdvancement();
            });
        }

        #region Custom animations
            protected IEnumerator StartDialogueAnimation()
            {
                yield break;
            }

            protected IEnumerator EndDialogueAnimation()
            {
                yield break;
            }
        #endregion
    }
}