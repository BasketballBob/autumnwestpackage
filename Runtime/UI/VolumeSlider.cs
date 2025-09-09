using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace AWP
{
    public class VolumeSlider : AWSlider
    {
        [SerializeField]
        private AudioManager.VolumeType _volumeType;
        [SerializeField]
        private EventReference _testSFX;

        private void Start()
        {
            _slider.value = AudioManager.GetVolume(_volumeType);
        }

        protected override void OnSliderChange(float newValue)
        {
            base.OnSliderChange(newValue);

            AudioManager.SetVolume(_volumeType, newValue);
            AWGameManager.AudioManager.PlayOneShot(_testSFX);
        }
    }
}
