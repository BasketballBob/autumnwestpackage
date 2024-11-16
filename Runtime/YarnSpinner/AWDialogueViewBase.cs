using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;

namespace AWP
{
    public class AWDialogueViewBase : DialogueViewBase
    {
        protected const float DefaultCharPrintDelay = .02f;

        protected LocalizedLine _prevLine;
        protected Coroutine _animationRoutine;

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
            _animationRoutine = StartCoroutine(routine);
        }   

        protected void StopAnimationRoutine()
        {
            if (_animationRoutine == null) return;
            StopCoroutine(_animationRoutine);
        }
    }
}