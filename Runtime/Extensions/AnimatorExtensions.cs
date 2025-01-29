using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AnimatorExtensions
    {
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
