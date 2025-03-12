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

        public TMP_Text Text => _text;
        public TMP_Text NameText => _nameText;
        protected float DismissAnimationDuration => .25f;

        protected override void OnEnable()
        {
            base.OnEnable();

            //SetHidden(true);
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
                while (Paused) yield return null;
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
                string bodyText = dialogueLine.TextWithoutCharacterName.Text;
                string nameText = dialogueLine.CharacterName;

                bool typewriterBody = _useTypewriterEffect;
                bool typewriterName = false; //_useTypewriterEffect;

                if (!typewriterBody) InstantPrintText(_text, bodyText);
                if (!typewriterName) InstantPrintText(_nameText, nameText);

                _canvasGroup.alpha = 1;

                yield return this.WaitOnRoutines(new IEnumerator[] {
                    typewriterBody ? PrintText(_text, bodyText) : null,
                    typewriterName ? PrintText(_nameText, nameText) : null
                });
            }

            protected virtual IEnumerator DismissLineAnimation()
            {
                yield return this.WaitOnRoutines(new IEnumerator[]
                {
                    _text.ShiftAlpha(0, DismissAnimationDuration, EasingFunction.Sin, AWDelta.DeltaType.Update),
                    _nameText.ShiftAlpha(0, 0, EasingFunction.Sin, AWDelta.DeltaType.Update)
                });

                _text.text = "";
                if (_nameText != null) _nameText.text = "";
            }
        #endregion
    }
}
