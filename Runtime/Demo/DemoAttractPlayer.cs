using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Video;
using FMOD.Studio;

namespace AWP
{
    public class DemoAttractPlayer : MonoBehaviour
    {
        public static DemoAttractPlayer Current { get; private set; }

        [SerializeField]
        private VideoPlayer _videoPlayer;

        private EventInstance _audioInstance;

        private void OnEnable()
        {
            if (Current == null) Current = this;
        }

        private void OnDisable()
        {
            if (Current == this) Current = null;
        }

        public IEnumerator PlayVideoLooped(VideoClip clip, EventReference clipAudio)
        {
            while (true)
            {
                yield return PlayVideo(clip, clipAudio);
            }
        }

        private IEnumerator PlayVideo(VideoClip clip, EventReference clipAudio)
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Play();

            if (!clipAudio.IsNull)
            {
                _audioInstance = AWGameManager.AudioManager.CreateInstance(clipAudio);
                _audioInstance.start();
            }

            while (_videoPlayer.isPlaying)
            {
                yield return null;
            }

            if (!clipAudio.IsNull) _audioInstance.DisposeOfSelf();
        }
    }
}
