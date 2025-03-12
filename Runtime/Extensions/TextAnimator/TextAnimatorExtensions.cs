using System.Collections;
using System.Collections.Generic;
using Febucci.UI.Core;
using UnityEngine;

namespace AWP
{
    public static class TextAnimatorExtensions
    {
        public static IEnumerator WaitUntilTextShown(this TypewriterCore typewriter)
        {
            bool waiting = true;
            typewriter.onTextShowed.AddListener(() => waiting = false);
            while (waiting) yield return null;
        }
    }
}
