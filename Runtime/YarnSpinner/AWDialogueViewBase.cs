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
        public virtual bool Paused { get; set; }

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