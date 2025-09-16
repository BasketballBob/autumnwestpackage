using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class ResolutionDropdown : AWDropdown<ResolutionDropdown.Resolution>
    {
        protected override LabeledList<Resolution> ItemList => new LabeledList<Resolution>()
        {
            {"1920 x 1080", new Resolution(1920, 1080) },
            {"1600 x 900", new Resolution(1600, 900) },
            {"640 x 360", new Resolution(640, 360) }
        };

        protected override void OnValueChanged(Resolution newValue)
        {
            Screen.SetResolution(newValue.Width, newValue.Height, Screen.fullScreenMode);
        }

        protected override Resolution GetStartValue() => new Resolution(Screen.width, Screen.height);

        [System.Serializable]
        public struct Resolution
        {
            public int Width;
            public int Height;

            public Resolution(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }
    }
}
