using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace AWP
{
    public class VolumeSlider : AWSlider
    {
        private const float TestSoundDelay = .2f;

        [SerializeField]
        private AudioManager.VolumeType _volumeType;
        [SerializeField]
        private EventReference _testSFX;

        private Alarm _testSoundDelay = new Alarm(TestSoundDelay);


        protected override void OnEnable()
        {
            base.OnEnable();

            _testSoundDelay.SetRemainingTime(0);
        }

        private void Start()
        {
            _slider.value = AudioManager.GetVolume(_volumeType);
        }

        protected override void OnSliderChange(float newValue)
        {
            base.OnSliderChange(newValue);

            AudioManager.SetVolume(_volumeType, newValue);
            CheckToPlayTestSFX();
        }

        private void CheckToPlayTestSFX()
        {
            Debug.Log($"CHECK TO PLAY TEST SFX {name} {_testSoundDelay.Delta}");

            if (!_testSoundDelay.IsFinished()) return;

            AWGameManager.AudioManager.PlayOneShot(_testSFX);

            _testSoundDelay.Reset();
            StartCoroutine(_testSoundDelay.RunUntilFinishRoutine(AWDelta.DeltaType.UnscaledUpdate));
        }
    }
}
