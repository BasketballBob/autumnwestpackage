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

        [Header("Music")]
        [SerializeField]
        private EventReference _currentMusic;
        [SerializeField]
        private EventInstance _musicInstance;

        [Header("Ambience")]
        [SerializeField]
        private EventReference _currentAmbience;
        [SerializeField]
        private EventInstance _ambienceInstance;

        [Header("Snapshot")]
        [SerializeField]
        private EventReference _currentSnapshot;
        [SerializeField]
        private EventInstance _snapshotInstance;

        private List<EventInstance> _eventList;
        private List<StudioEventEmitter> _emitterList;
        private Coroutine _musicFadeRoutine;
        private Coroutine _ambienceFadeRoutine;

        public static AudioManager Current { get; private set; }
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
        }

        private void OnDestroy()
        {
            _bankLoader.Unload();

            _musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _snapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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

        public void SetGlobalLabeledParameterByName(string name, string label)
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

        public void OnBeginLoadScene(string nextScene, float fadeDuration = DefaultFadeInDuration)
        {
            
        }

        public void OnSceneLoaded(string currentLevel)
        {

        }

        private void UpdateVolumes()
        {
            _masterBus.setVolume(MasterVolume);
            _sfxBus.setVolume(SFXVolume);
            _musicBus.setVolume(MusicVolume);
        }

        public void PlayMusic(EventReference eventRef, float fadeDuration = DefaultFadeInDuration)
        {
            if (eventRef.Guid == _currentMusic.Guid) return;

            _musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _musicInstance.release();
            _currentMusic = eventRef;
            

            if (eventRef.IsNull) return;
            _musicInstance = CreateEvent(eventRef);
            _musicInstance.start();
            _musicInstance.setVolume(0);
            // FadeMusicSFX
        }

       
    }
}
