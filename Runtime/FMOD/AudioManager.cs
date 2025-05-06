using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AWP
{
    public class AudioManager : MonoBehaviour
    {
        private const float DefaultVolume = 1;
        private const float DefaultFadeOutDuration = .3f;
        private const float DefaultFadeInDuration = .3f;

        [Header("References")]
        [SerializeField]
        private StudioBankLoader _bankLoader;
        //[SerializeField]
        //private AudioDataSetHolder _masterAudioSet;

        [Header("Volumes")]
        [Range(0, 1)]
        public static float MasterVolume = 1;
        [Range(0, 1)]
        public static float SFXVolume = 1;
        [Range(0, 1)]
        public static float MusicVolume = 1;
        [Range(0, 1)]
        public static float AmbienceVolume = 1;

        private Bus _masterBus;
        private Bus _sfxBus;
        private Bus _musicBus;

        [Header("Audio Channels")]
        [SerializeField]
        private AudioChannel _musicChannel;
        [SerializeField]
        private AudioChannel _ambienceChannel;
        [SerializeField]
        private AudioChannel _snapshotChannel;

        [Header("Scene Audio")]
        [SerializeField]
        private SceneAudioMode _sceneAudioMode;

        private List<EventInstance> _eventList;
        private List<StudioEventEmitter> _emitterList;
        private SceneAudio _currentSceneAudio;

        public static AudioManager Current { get; private set; }
        public enum SceneAudioMode { Auto, SceneController, None };
        public enum EventPlayType { Music, Ambience, Snapshot };

        private void Awake()
        {
            Current = this;
            //DontDestroyOnLoad(gameObject);

            _bankLoader.Load();

            _masterBus = RuntimeManager.GetBus("bus:/MasterBus");
            _sfxBus = RuntimeManager.GetBus("bus:/MasterBus/SFX");
            _musicBus = RuntimeManager.GetBus("bus:/masterBus/Music");

            _eventList = new List<EventInstance>();
            _emitterList = new List<StudioEventEmitter>();

            _musicChannel = new AudioChannel(this);
            _ambienceChannel = new AudioChannel(this);
            _snapshotChannel = new AudioChannel(this);
        }

        private void OnDestroy()
        {
            _bankLoader.Unload();

            _musicChannel.Clean();
            _ambienceChannel.Clean();
            _snapshotChannel.Clean();
        }

        public static void SetGlobalParameterByName(string name, float value)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, value);
        }

        public static float GetGlobalParameterByName(string name)
        {
            float value;
            FMODUnity.RuntimeManager.StudioSystem.getParameterByName(name, out value);
            return value;
        }

        public static void SetGlobalLabeledParameterByName(string name, string label)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel(name, label);
        }

        public void PlayOneShot(EventReference eventRef, Vector3 worldPos = default)
        {
            if (eventRef.IsNull) return;
            RuntimeManager.PlayOneShot(eventRef, worldPos);
        }

        public void PlayOneShotAttached(EventReference eventRef, GameObject gameObject)
        {
            if (eventRef.IsNull) return;
            RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        }

        private EventInstance CreateEvent(EventReference eventRef)
        {
            EventInstance instance = RuntimeManager.CreateInstance(eventRef);
            return instance;
        }

        public EventInstance CreateInstance(EventReference eventRef)
        {
            EventInstance instance = CreateEvent(eventRef);
            _eventList.Add(instance);
            return instance;
        }

        public EventInstance CreateAttachedInstance(EventReference eventRef, GameObject gameObject)
        {
            EventInstance instance = CreateInstance(eventRef);
            RuntimeManager.AttachInstanceToGameObject(instance, gameObject);
            return instance;
        }

        public StudioEventEmitter InitializeEventEmitter(StudioEventEmitter emitter, EventReference eventReference)
        {
            emitter.EventReference = eventReference;
            return InitializeEventEmitter(emitter);
        }

        public StudioEventEmitter InitializeEventEmitter(StudioEventEmitter emitter)
        {
            _emitterList.Add(emitter);
            return emitter;
        }

        /// <summary>
        /// Function called by GameManager OnSceneLoaded to cleanup the audio from the previous level
        /// </summary>
        public static void CleanUp()
        {
            for (int i = 0; i < Current._eventList.Count; i++)
            {
                if (!Current._eventList[i].isValid())
                {
                    Current._eventList.RemoveAt(i);
                    i--;
                    continue;
                }

                Current._eventList[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                Current._eventList[i].release();
            }

            for (int i = 0; i < Current._emitterList.Count; i++)
            {
                if (Current._emitterList[i] == null)
                {
                    Current._emitterList.RemoveAt(i);
                    i--;
                    continue;
                }

                Current._emitterList[i].Stop();
            }
        }

        // public void OnBeginLoadScene(string nextScene, float fadeDuration = DefaultFadeInDuration)
        // {
        //     PrepareForNewSceneAudio(nextScene, fadeDuration);
        // }

        // public void OnSceneLoaded(string currentLevel)
        // {
        //     LoadNewSceneAudio(currentLevel);
        // }

        private void UpdateVolumes()
        {
            _masterBus.setVolume(MasterVolume);
            _sfxBus.setVolume(SFXVolume);
            _musicBus.setVolume(MusicVolume);
        }

        [Button()]
        public void PlayMusic(EventReference eventRef, float fadeDuration = DefaultFadeInDuration, float volume = DefaultVolume) =>
            _musicChannel.PlayEvent(eventRef, fadeDuration, volume);

        [Button()]
        public void PlayAmbience(EventReference eventRef, float fadeDuration = DefaultFadeInDuration, float volume = DefaultVolume) =>
            _ambienceChannel.PlayEvent(eventRef, fadeDuration, volume);

        #region Scene Audio
            public void EnterNewSceneAudio(SceneAudio sceneAudio, float fadeDuration = DefaultFadeInDuration)
            {
                _musicChannel.PlayEvent(sceneAudio.Music.EventReference, fadeDuration);
                _ambienceChannel.PlayEvent(sceneAudio.Ambience.EventReference, fadeDuration);

                // StartCoroutine(this.WaitOnRoutines(new IEnumerator[] 
                // {
                //     _musicChannel.PlayEvent(sceneAudio.Music.EventReference, fadeDuration),
                //     _ambienceChannel.PlayEvent(sceneAudio.Ambience.EventReference, fadeDuration),
                //     //_snapshotChannel.PlayEvent(sceneAudio.Snapshot)
                // }));
            }
            // private void PrepareForNewSceneAudio(string scene, float fadeDuration)
            // {
            //     if (!_useSceneAudio) return;

            //     SceneAudio newAudio = SceneAudio.LoadSceneAudio(scene);
            //     if (newAudio == null) return;
            //     if (newAudio == _currentSceneAudio) return;

            //     if (newAudio.Music != _currentSceneAudio.Music) 
            //         _musicRoutine.StartRoutine(FadeMusic(0, fadeDuration));
            //     if (newAudio.Ambience != _currentSceneAudio.Ambience)
            //         _ambienceRoutine.StartRoutine(FadeAmbience(0, fadeDuration));
            // }

            // private void LoadNewSceneAudio(string scene)
            // {
            //     if (!_useSceneAudio) return;

            //     SceneAudio newAudio = SceneAudio.LoadSceneAudio(scene);
            //     if (newAudio == null) return;
            //     if (newAudio == _currentSceneAudio) return;

            //     if (newAudio.Music != _currentSceneAudio.Music)
            //         PlayMusic(newAudio.Music.EventReference, newAudio.Music.Volume);
            //     if (newAudio.Ambience != _currentSceneAudio.Ambience)
            //         PlayAmbience(newAudio.Ambience.EventReference, newAudio.Ambience.Volume);
            // }
        #endregion

        #region Audio channel
        [System.Serializable]
        private class AudioChannel
        {
            public EventReference CurrentEvent;
            public EventInstance Instance;

            private AudioManager _audioManager;
            private SingleCoroutine _shiftRoutine;

            public AudioChannel(AudioManager manager)
            {
                _audioManager = manager;
                _shiftRoutine = new SingleCoroutine(manager);
            }

            /// <summary>
            /// Cleans the channel once the game is done using it
            /// </summary>
            public void Clean()
            {
                Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                Instance.release();
            }

            public void PlayEvent(EventReference eventRef, float fadeDuration = DefaultFadeInDuration, float volume = DefaultVolume)
            {
                _shiftRoutine.StartRoutine(SwitchAudio(eventRef, fadeDuration, volume));
            }
            public void PlaySceneAudioSettings(SceneAudio.SceneAudioSettings settings, float fadeDuration = DefaultFadeInDuration)
            {
                PlayEvent(settings.EventReference, fadeDuration, settings.Volume);
            }

            private IEnumerator SwitchAudio(EventReference eventRef, float fadeDuration = DefaultFadeInDuration, float volume = DefaultVolume)
            {
                // Fade to new volume if eventRefs are the same
                if (!CurrentEvent.IsNull && eventRef.Guid == CurrentEvent.Guid)
                {
                    yield return Instance.FadeToVolume(fadeDuration, volume);
                    yield break;
                }

                // Fade out old audio
                if (Instance.isValid())
                {
                    yield return Instance.FadeToVolume(fadeDuration / 2, 0);
                    Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    Instance.release();
                    CurrentEvent = default;
                }

                // Fade in new audio
                if (!eventRef.IsNull)
                {
                    Instance = _audioManager.CreateEvent(eventRef);
                    CurrentEvent = eventRef;
                    Instance.start();
                    Instance.setVolume(0);
                    yield return Instance.FadeToVolume(1, fadeDuration / 2);
                }
            }
        }
        #endregion
    }
}
