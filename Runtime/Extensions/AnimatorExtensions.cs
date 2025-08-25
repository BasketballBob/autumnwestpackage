using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AnimatorExtensions
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

        public static IEnumerator WaitForCrossFadeInFixedTime(this Animator anim, string stateName, float fixedDuration)
        {
            anim.CrossFadeInFixedTime(stateName, fixedDuration);
            yield return new WaitForSeconds(fixedDuration);
        }

        /// <summary>
        /// Plays animation at specific duration and waits for completion
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        /// <param name="duration"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static IEnumerator WaitForAnimationToCompleteAtDuration(this Animator anim, string animName, float duration, int layer = 0)
        {
            anim.Play(animName, layer);
            anim.SetSpeedForDuration(duration);
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
        
        public static AnimatorClipInfo[] GetUpdatedCurrentClipInfo(this Animator animator, int layer)
        {
            animator.Update(0);
            return animator.GetCurrentAnimatorClipInfo(0);
        }

        public static float GetCurrentClipScaledDuration(this Animator animator, int layer)
        {
            return animator.GetUpdatedCurrentClipInfo(0)[0].clip.length * animator.speed;
        }

        public static void ResetSpeed(this Animator animator) => animator.speed = 1;

        public static void SetSpeedForDuration(this Animator animator, float duration, int layerIndex = 0)
        {
            animator.speed = animator.GetUpdatedCurrentClipInfo(layerIndex)[0].clip.length / duration;
        }

        public static void ClampSpeedForMaxDuration(this Animator animator, float maxDuration, int layerIndex = 0)
        {
            if (animator.GetCurrentClipScaledDuration(layerIndex) < maxDuration) return;
            SetSpeedForDuration(animator, maxDuration, layerIndex);
        }

        public static void ClampSpeedForMinDuration(this Animator animator, float minDuration, int layerIndex = 0)
        {
            if (animator.GetCurrentClipScaledDuration(layerIndex) > minDuration) return;
            SetSpeedForDuration(animator, minDuration, layerIndex);
        }

        public static void ClampSpeedDuration(this Animator animator, float minDuration, float maxDuration, int layerIndex = 0)
        {
            float currentDuration = animator.GetCurrentClipScaledDuration(layerIndex);
            if (currentDuration > maxDuration) animator.SetSpeedForDuration(maxDuration);
            if (currentDuration < minDuration) animator.SetSpeedForDuration(minDuration);
        }
    }   
}
