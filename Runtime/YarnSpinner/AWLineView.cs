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
        private bool _waitForInput = true;
        [SerializeField]
        private bool _useTypewriterEffect = true;

        protected float DismissAnimationDuration => .5f;

        public override void DialogueStarted()
        {
            base.DialogueStarted();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _advanceHandler = requestInterrupt;
            StartAnimationRoutine(RunLineRoutine());
            
            IEnumerator RunLineRoutine()
            {
                yield return RunLineAnimation(dialogueLine);
                if (!_waitForInput) onDialogueLineFinished?.Invoke();
            }
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            if (AnimationActive)
            {
                StopAnimationRoutine();
                InstantPrintText(_text, dialogueLine.TextWithoutCharacterName.Text);
                return;
            }

            _advanceHandler = null;
            onDialogueLineFinished();
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            StartAnimationRoutine(DismissLineRoutine());

            IEnumerator DismissLineRoutine()
            {
                yield return DismissLineAnimation();
                onDismissalComplete?.Invoke();
            }
        }

        public override void DialogueComplete()
        {
            
        }

        #region Animations
            protected virtual IEnumerator RunLineAnimation(LocalizedLine dialogueLine)
            {
                _canvasGroup.alpha = 1;
                if (_useTypewriterEffect) yield return PrintText(_text, dialogueLine.TextWithoutCharacterName.Text);
            }

            protected virtual IEnumerator DismissLineAnimation()
            {
                yield return _canvasGroup.CanvasGroupShiftAlpha(0, DismissAnimationDuration, EasingFunction.Sin, AWDelta.DeltaType.Update);
            }
        #endregion
    }
}
