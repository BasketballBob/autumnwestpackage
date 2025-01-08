using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [System.Serializable] [InlineProperty]
    public struct ShiftSettings
    {
        [HorizontalGroup("Main")] [HideLabel]
        public float Duration;
        [HorizontalGroup("Main")] [HideLabel]
        public EasingFunction EasingMode;

        public ShiftSettings(float duration = .5f)
        {
            Duration = duration;
            EasingMode = EasingFunction.Sin;
        }

        public static ShiftSettings Default => new ShiftSettings();
    }
}
