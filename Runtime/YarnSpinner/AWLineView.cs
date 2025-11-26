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
        private TMP_Text _textTMP;
        [SerializeField]
        private TypewriterCore _nameText;
        [SerializeField]
        private TMP_Text _nameTMP;
        [SerializeField]
        private Color _optionsTextColor = Color.black;

        [Header("Text Printing")]
        [SerializeField]
        private bool _waitForInput = true;

        public Action OnLineStart;
        public Action OnLineFinish;
        protected bool _enterBodyText;
        protected bool _enterNameText;

        public TMP_Text TextTMP => _textTMP;
        public TMP_Text NameTMP => _nameTMP;
        public TypewriterCore TextTypewriter => _text;
        public TypewriterCore NameTypewriter => _nameText;
        public bool WaitingOnInput { get; private set; }
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
                OnLineStart?.Invoke();

                while (Paused) yield return null;

                yield return RunLineAnimation(dialogueLine);

                if (!_waitForInput) onDialogueLineFinished?.Invoke();
                else WaitingOnInput = true;

                OnLineFinish?.Invoke();
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
            WaitingOnInput = false;
            StartAnimationRoutine(DismissLineRoutine());

            IEnumerator DismissLineRoutine()
            {
                yield return DismissLineAnimation();
                onDismissalComplete?.Invoke();
            }
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            base.RunOptions(dialogueOptions, onOptionSelected);

            _text.hideAppearancesOnSkip = true;
            if (_nameText != null) _nameText.hideAppearancesOnSkip = true;

            RunLine(_prevLine, null);

            _textTMP.color = _optionsTextColor;
            _nameTMP.color = _optionsTextColor;

            _text.SkipTypewriter();
            if (_nameText != null) _nameText.SkipTypewriter();
            _text.hideAppearancesOnSkip = false;
            if (_nameText != null) _nameText.hideAppearancesOnSkip = false;
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
            // yield return this.WaitOnRoutines(new IEnumerator[]
            // {
            //     _text.DisappearText(),
            //     _nameText != null ? _nameText.DisappearText() : null
            // });

            yield return null; //DEBUG
            _text.ShowText("");
            if (_nameText != null) _nameText.ShowText("");
        }
        #endregion
    }
}
