using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;
using Sirenix.Utilities;

namespace AWP
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AWDialogueViewBase : DialogueViewBase
    {
        protected const float DefaultCharPrintDelay = .014f;

        protected CanvasGroup _canvasGroup;
        protected LocalizedLine _prevLine;
        protected Action _advanceHandler;
        protected Coroutine _animationRoutine;

        public bool AnimationActive => _animationRoutine != null;

        protected virtual void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // public override void DialogueStarted()
        // {
            
        // }

        public override void UserRequestedViewAdvancement()
        {
            _advanceHandler?.Invoke();
        }

        public virtual void SetHidden(bool hidden)
        {
            if (hidden) _canvasGroup.alpha = 0;
        }

        protected IEnumerator PrintText(TMP_Text tmp, string text, float charDelay = DefaultCharPrintDelay)
        {
            if (tmp == null) yield break;
            if (string.IsNullOrEmpty(text)) text = "";

            tmp.maxVisibleCharacters = 0;
            tmp.text = text;
            tmp.ForceMeshUpdate();

            while (tmp.maxVisibleCharacters < tmp.text.Length)
            {
                tmp.maxVisibleCharacters++;
                if (tmp.maxVisibleCharacters < tmp.text.Length)
                    yield return new WaitForSeconds(charDelay);
            }
        }

        protected void InstantPrintText(TMP_Text tmp, string text)
        {
            if (tmp == null) return;
            if (string.IsNullOrEmpty(text)) text = "";

            tmp.maxVisibleCharacters = text.Length;
            tmp.text = text;
            tmp.ForceMeshUpdate();
        }

        /// <summary>
        /// Starts a tracked routine that represents the current primary animation
        /// </summary>
        /// <param name="routine">Routine to call</param>
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