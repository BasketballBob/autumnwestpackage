using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AnimationEffects
    {
        public static IEnumerator WaitForAnimationToComplete(Animator anim)
        {
            yield return null;

            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
        }
    }
}
