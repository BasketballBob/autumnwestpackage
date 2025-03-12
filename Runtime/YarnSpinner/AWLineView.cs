using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;
using Febucci.UI;
using Febucci.UI.Core;

namespace AWP
{
    public class AWLineView : AWDialogueViewBase
    {
        [SerializeField]
        private TypewriterCore _text;
        [SerializeField]
        private TypewriterCore _nameText;

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
                _text.SkipTypewriter();
                if (_nameText != null) _nameText.SkipTypewriter();
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

                _canvasGroup.alpha = 1;

                _text.ShowText(bodyText);
                if (_nameText != null) _nameText.ShowText(nameText);

                yield return this.WaitOnRoutines(new IEnumerator[] {
                    _text.WaitUntilTextShown(),
                    _nameText != null ? _nameText.WaitUntilTextShown() : null
                });
            }

            protected virtual IEnumerator DismissLineAnimation()
            {
                yield return this.WaitOnRoutines(new IEnumerator[]
                {
                    //_text.ShiftAlpha(0, DismissAnimationDuration, EasingFunction.Sin, AWDelta.DeltaType.Update),
                    //_nameText.ShiftAlpha(0, 0, EasingFunction.Sin, AWDelta.DeltaType.Update)
                });

                _text.ShowText("");
                if (_nameText != null) _nameText.ShowText("");
            }
        #endregion
    }
}
