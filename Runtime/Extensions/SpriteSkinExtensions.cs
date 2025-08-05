using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace AWP
{
    public static class SpriteSkinExtensions
    {
        public static void SetBoneTransforms(this SpriteSkin spriteSkin, Transform[] newBones)
        {
            var rootBoneProperty = typeof(SpriteSkin).GetProperty(nameof(SpriteSkin.rootBone));
            var boneTransformsProperty = typeof(SpriteSkin).GetProperty(nameof(SpriteSkin.boneTransforms));

            rootBoneProperty!.SetValue(spriteSkin, newBones[0]);
            boneTransformsProperty!.SetValue(spriteSkin, newBones);
        }
    }
}
