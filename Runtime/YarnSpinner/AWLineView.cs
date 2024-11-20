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
        [SerializeField]
        private TMP_Text _nameText;

        [Header("Text Printing")]
        [SerializeField]
        private bool _waitForInput = true;
        [SerializeField]
        private bool _useTypewriterEffect = true;

        protected bool _enterBodyText;
        protected bool _enterNameText;

        protected float DismissAnimationDuration => .25f;

        protected override void OnEnable()
        {
            base.OnEnable();

            SetHidden(true);
        }

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
                _prevLine = dialogueLine;
            }
        }

        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            if (AnimationActive)
            {
                StopAnimationRoutine();
                InstantPrintText(_text, dialogueLine.TextWithoutCharacterName.Text);
                InstantPrintText(_nameText, dialogueLine.CharacterName);
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
                bool typewriterBody = _useTypewriterEffect;
                bool typewriterName = false; //_useTypewriterEffect;

                _canvasGroup.alpha = 1;

                yield return this.WaitOnRoutines(new IEnumerator[] {
                    typewriterBody ? PrintText(_text, dialogueLine.TextWithoutCharacterName.Text) : null,
                    typewriterName ? PrintText(_nameText, dialogueLine.CharacterName) : null
                });
            }

            protected virtual IEnumerator DismissLineAnimation()
            {
                yield return this.WaitOnRoutines(new IEnumerator[]
                {
                    _text.ShiftAlpha(0, DismissAnimationDuration, EasingFunction.Sin, AWDelta.DeltaType.Update),
                   // _nameText.ShiftAlpha(0, DismissAnimationDuration, EasingFunction.Sin, AWDelta.DeltaType.Update)
                });
            }
        #endregion
    }
}
