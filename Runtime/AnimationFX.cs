using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AWP
{
    public static class AnimationFX
    {
        public static IEnumerator WaitForAnimationToComplete(this Animator anim, int layer = 0)
        {
            yield return null;

            while (anim.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1)
            {
                yield return null;
            }
        }
        public static IEnumerator WaitForAnimationToComplete(this Animator anim, string animName, int layer = 0)
        {
            anim.Play(animName, layer);
            return anim.WaitForAnimationToComplete(layer);
        }

        public static IEnumerator WaitForTransitionToFinish(this Animator anim, int layer = 0)
        {
            // Wait for transition to start
            anim.Update(0);
            while (!anim.IsInTransition(layer)) yield return null;

            // Wait for transition to end
            while (anim.IsInTransition(layer))
            {
                yield return null;
            }
        }

        #region Canvas group
            public static IEnumerator ShiftAlpha(this CanvasGroup canvasGroup, float endAlpha, float duration, EasingFunction easingFunc, AWDelta.DeltaType deltaType = AWDelta.DeltaType.Update)
            {
                float startAlpha = canvasGroup.alpha;

                return DeltaRoutine((delta) => 
                {
                    canvasGroup.alpha = startAlpha + (endAlpha - startAlpha) * delta;
                }, duration, easingFunc, deltaType);
            }
        #endregion

        #region TMP_Text
            public static IEnumerator ShiftAlpha(this TMP_Text tmp,  float endAlpha, float duration, EasingFunction easingFunc, AWDelta.DeltaType deltaType = AWDelta.DeltaType.Update)
            {
                float startAlpha = tmp.color.a;

                return DeltaRoutine((delta) => 
                {
                    tmp.color.SetAlpha(startAlpha + (endAlpha - startAlpha) * delta);
                }, duration, easingFunc, deltaType);
            }
        #endregion 

        #region Helper functions
            public static IEnumerator DeltaRoutine(Action<float> deltaAction, float duration, EasingFunction easingFunc, AWDelta.DeltaType deltaType = AWDelta.DeltaType.Update)
            {
                Alarm alarm = new Alarm(duration);

                while (alarm.RunWhileUnfinished(deltaType.GetDelta()))
                {
                    deltaAction(easingFunc.GetEasedDelta(alarm.Delta));
                    yield return deltaType.YieldNull();
                }

                deltaAction(easingFunc.GetEasedDelta(1));
            }
        #endregion
    }
}
