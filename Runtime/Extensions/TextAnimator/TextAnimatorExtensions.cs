using System.Collections;
using System.Collections.Generic;
using Febucci.UI.Core;
using UnityEngine;

namespace AWP
{
    public static class TextAnimatorExtensions
    {
        /// <summary>
        /// Waits for text to finish displaying 
        /// </summary>
        /// <param name="typewriter"></param>
        /// <returns></returns>
        public static IEnumerator WaitUntilTextShown(this TypewriterCore typewriter)
        {
            bool waiting = true;
            typewriter.onTextShowed.AddListener(() => waiting = false);
            while (waiting) yield return null;
        }

        /// <summary>
        /// Waits for text to finish disappearing
        /// </summary>
        /// <param name="typewriter"></param>
        /// <returns></returns>
        public static IEnumerator WaitUntilTextDisappeared(this TypewriterCore typewriter)
        {
            while (typewriter.isHidingText) yield return null;
        }

        /// <summary>
        /// Disappears the text and waits for it to finish
        /// </summary>
        /// <param name="typewriter"></param>
        /// <returns></returns>
        public static IEnumerator DisappearText(this TypewriterCore typewriter)
        {
            typewriter.StartDisappearingText();
            yield return WaitUntilTextDisappeared(typewriter);
        }

        public static void SkipTypewriterInstant(this TypewriterCore typewriter)
        {
            bool hideAppearancesOnSkip = typewriter.hideAppearancesOnSkip;
            typewriter.hideAppearancesOnSkip = true;

            typewriter.SkipTypewriter();

            typewriter.hideAppearancesOnSkip = hideAppearancesOnSkip;
        }

        public static void ShowInstant(this TypewriterCore typewriter, string text)
        {
            typewriter.ShowText(text);
            typewriter.SkipTypewriterInstant();
        }
    }
}
