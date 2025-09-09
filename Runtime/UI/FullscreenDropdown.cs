using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class FullscreenDropdown : AWDropdown<FullScreenMode>
    {
        protected override LabeledList<FullScreenMode> ItemList => new LabeledList<FullScreenMode>()
        {
            {"Windowed", FullScreenMode.Windowed},
            {"Fullscreen", FullScreenMode.FullScreenWindow},
            {"Maximized", FullScreenMode.MaximizedWindow},
            {"Exclusive Fullscreen", FullScreenMode.ExclusiveFullScreen}
        };

        protected override void OnValueChanged(FullScreenMode newValue)
        {
            Screen.fullScreenMode = newValue;
        }

        protected override FullScreenMode GetStartValue() => Screen.fullScreenMode;
    }
}
