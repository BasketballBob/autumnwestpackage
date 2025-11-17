using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;

namespace AWP
{
    public class AudioManager : MonoBehaviour
    {
        private const float DefaultVolume = 1;
        private const float DefaultFadeInDuration = .3f;
        private const float DefaultFadeOutDuration = .3f;

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

        [Header("Debug")]
        [ShowInInspector]
        private SceneAudio _lastLoadedSceneAudio;

        private List<EventInstance> _eventList;
        private List<StudioEventEmitter> _emitterList;

        public static AudioManager Current { get; private set; }
        public enum VolumeType { Master, SFX, Music, Ambience }
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
        public static string GetGlobalLabeledParameterByName(string name)
        {
            int index = (int)GetGlobalParameterByName(name);
            string value;
            FMODUnity.RuntimeManager.StudioSystem.getParameterLabelByName(name, index, out value);
            return value;
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

                Current._eventList[i].DisposeOfSelf();
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
        public void PlayMusic(EventReference eventRef, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, float volume = DefaultVolume) =>
            _musicChannel.PlayEvent(eventRef, fadeEnter, fadeExit, volume);

        [Button()]
        public void PlayAmbience(EventReference eventRef, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, float volume = DefaultVolume) =>
            _ambienceChannel.PlayEvent(eventRef, fadeEnter, fadeExit, volume);

        [Button()]
        public void PlaySnapshot(EventReference eventRef, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, float volume = DefaultVolume) =>
            _snapshotChannel.PlayEvent(eventRef, fadeEnter, fadeExit, volume);

        #region Scene Audio
        /// <summary>
        /// Gets the scene audio for the target scene (can also decide whether or not it is used)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SceneAudio GetSceneAudio(string name)
        {
            SceneAudio audio = SceneAudio.LoadSceneAudio(name);
            return audio;
        }

        public void EnterNewSceneAudio(SceneAudio sceneAudio, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
        {
            Debug.Log($"ENTER NEW SCENE AUDIO {sceneAudio.name}");
            _lastLoadedSceneAudio = sceneAudio;

            sceneAudio.ApplyGlobalParameters();
            _musicChannel.PlayEvent(sceneAudio.Music.EventReference, fadeEnter, fadeExit, sceneAudio.Music.Volume, onSwitch);
            _ambienceChannel.PlayEvent(sceneAudio.Ambience.EventReference, fadeEnter, fadeExit, sceneAudio.Ambience.Volume, onSwitch);
            _snapshotChannel.PlayEvent(sceneAudio.Snapshot.EventReference, fadeEnter, fadeExit, sceneAudio.Snapshot.Volume, onSwitch);
        }
        public void EnterNewSceneAudio(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration) =>
            EnterNewSceneAudio(name, fadeEnter, fadeExit, null);
        public void EnterNewSceneAudio(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null) =>
            EnterNewSceneAudio(GetSceneAudio(name), fadeEnter, fadeExit, onSwitch);

        public void PlaySceneAudioMusic(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
        {
            SceneAudio sceneAudio = GetSceneAudio(name);
            _musicChannel.PlayEvent(sceneAudio.Music.EventReference, fadeEnter, fadeExit, sceneAudio.Music.Volume, onSwitch);
        }
        public void PlaySceneAudioAmbience(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
        {
            SceneAudio sceneAudio = GetSceneAudio(name);
            _ambienceChannel.PlayEvent(sceneAudio.Ambience.EventReference, fadeEnter, fadeExit, sceneAudio.Ambience.Volume, onSwitch);
        }
        public void PlaySceneAudioSnapshot(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
        {
            SceneAudio sceneAudio = GetSceneAudio(name);
            _snapshotChannel.PlayEvent(sceneAudio.Snapshot.EventReference, fadeEnter, fadeExit, sceneAudio.Snapshot.Volume, onSwitch);
        }
        #endregion

        #region Volumes
        public static void SetVolume(VolumeType volumeType, float newValue)
        {
            newValue = Mathf.Clamp01(newValue);
            GetVolumeRef(volumeType) = newValue;
            Current.UpdateVolumes();
        }
        public static float GetVolume(VolumeType volumeType) => GetVolumeRef(volumeType);

        private static ref float GetVolumeRef(VolumeType volumeType)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    return ref MasterVolume;
                case VolumeType.SFX:
                    return ref SFXVolume;
                case VolumeType.Music:
                    return ref MusicVolume;
                case VolumeType.Ambience:
                    return ref AmbienceVolume;
            }

            throw new Exception("VOLUME TIME DOESN'T EXIST!");
        }
        #endregion

        #region Audio channels
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
                Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                Instance.release();
            }

            public void PlayEvent(EventReference eventRef, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, float volume = DefaultVolume, Action onSwitch = null, List<AWEventParameter> localParams = null)
            {
                _shiftRoutine.StartRoutine(SwitchAudio(eventRef, fadeEnter, fadeExit, volume, onSwitch, localParams));
            }
            public void PlaySceneAudioSettings(SceneAudio.SceneAudioSettings settings, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
            {
                PlayEvent(settings.EventReference, fadeEnter, fadeExit, settings.Volume, onSwitch);
            }

            /// <summary>
            /// Switches the track audio to the provided eventRef
            /// </summary>
            /// <param name="eventRef"></param>
            /// <param name="fadeEnter"></param>
            /// <param name="fadeExit"></param>
            /// <param name="volume"></param>
            /// <param name="onSwitch">Action to optionally be called to allow the entering of new audio</param>
            /// <returns></returns>
            private IEnumerator SwitchAudio(EventReference eventRef, float fadeEnter, float fadeExit, float volume = DefaultVolume, Action onSwitch = null, List<AWEventParameter> localParams = null)
            {
                bool readyToSwitch = false;
                InitializeOnSwitchEvent();

                //Debug.Log($"SWITCH AUDIO {CurrentEvent.IsNull} {eventRef.Path} {(!CurrentEvent.IsNull ? CurrentEvent.Path : null)}");

                // Fade to new volume if eventRefs are the same
                if (!CurrentEvent.IsNull && eventRef.Guid == CurrentEvent.Guid)
                {
                    yield return WaitOnSwitchEvent();
                    yield return Instance.FadeToVolume(fadeEnter + fadeExit, volume);
                    yield break;
                }

                // Fade out old audio
                if (Instance.isValid())
                {
                    yield return Instance.FadeToVolume(fadeEnter, 0);
                    Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    Instance.release();
                    CurrentEvent = default;
                }

                yield return WaitOnSwitchEvent();

                // Fade in new audio
                if (!eventRef.IsNull)
                {
                    Instance = _audioManager.CreateAttachedInstance(eventRef, AWGameManager.AWCamera.gameObject);
                    //Instance.setPa // HERE
                    CurrentEvent = eventRef;
                    Instance.start();
                    Instance.setVolume(0);
                    yield return Instance.FadeToVolume(fadeExit, volume);
                }

                void InitializeOnSwitchEvent()
                {
                    if (onSwitch == null)
                    {
                        readyToSwitch = true;
                        return;
                    }

                    onSwitch += () => readyToSwitch = true;
                }

                IEnumerator WaitOnSwitchEvent()
                {
                    while (!readyToSwitch) yield return null;
                }
            }
        }
        #endregion

        #region Yarn Commands
        [YarnCommand("EnterNewSceneAudio")]
        public static void YarnEnterNewSceneAudio(string name, float fadeEnter = DefaultFadeInDuration, float fadeExit = DefaultFadeOutDuration, Action onSwitch = null)
        {
            AWGameManager.AudioManager.EnterNewSceneAudio(name, fadeEnter, fadeExit, onSwitch);
        }
        #endregion
    }
}
