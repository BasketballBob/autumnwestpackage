using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;

namespace AWP
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AWDialogueViewBase : DialogueViewBase
    {
        protected const float DefaultCharPrintDelay = .02f;

        protected CanvasGroup _canvasGroup;
        protected LocalizedLine _prevLine;
        protected Action _advanceHandler;
        protected Coroutine _animationRoutine;

        public bool AnimationActive => _animationRoutine != null;

        protected virtual void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void DialogueStarted()
        {
            
        }

        public override void UserRequestedViewAdvancement()
        {
            _advanceHandler?.Invoke();
        }

        protected IEnumerator PrintText(TMP_Text tmp, string text, float charDelay = DefaultCharPrintDelay)
        {
            tmp.maxVisibleCharacters = 0;
            tmp.text = text;

            while (tmp.maxVisibleCharacters < tmp.text.Length)
            {
                tmp.maxVisibleCharacters++;
                if (tmp.maxVisibleCharacters < tmp.text.Length)
                    yield return new WaitForSeconds(charDelay);
            }
        }

        protected void InstantPrintText(TMP_Text tmp, string text)
        {
            tmp.maxVisibleCharacters = text.Length;
            tmp.text = text;
        }

        protected void StartAnimationRoutine(IEnumerator routine)
        {
            StopAnimationRoutine();
            _animationRoutine = StartCoroutine(AnimationRoutine());

            IEnumerator AnimationRoutine()
            {
                yield return routine;
                _animationRoutine = null;
            }
        }   

        protected void StopAnimationRoutine()
        {
            if (_animationRoutine == null) return;
            StopCoroutine(_animationRoutine);
            _animationRoutine = null;
        }
    }
}