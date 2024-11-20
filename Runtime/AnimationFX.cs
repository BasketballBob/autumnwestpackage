using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AnimationFX
    {
        public static IEnumerator WaitForAnimationToComplete(this Animator anim)
        {
            yield return null;

            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
        }
        public static IEnumerator WaitForAnimationToComplete(this Animator anim, string animName, int layer = 0)
        {
            anim.Play(animName, layer);
            return anim.WaitForAnimationToComplete();
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
