using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Sirenix.OdinInspector;
using FMOD.Studio;
using System;

namespace AWP
{
    [System.Serializable] [InlineProperty] [HideLabel]
    public abstract class AWEventInstance
    {
        [LabelText("@$property.ParentValueProperty.NiceName")]
        public EventReference Event;
        protected MonoBehaviour _mono;
        protected EventInstance _instance;
        protected bool _active;
        protected SingleCoroutine _updateRoutine;
        private bool _initialized;

        public EventInstance Instance => _instance;
        public bool InstanceValid => _instance.isValid();
        protected virtual Action<EventInstance> PlayEvent => (x) => x.StartIfNotPlaying();
        protected virtual Action<EventInstance> StopEvent => (x) => x.StopIfPlaying();

        public void Initialize(MonoBehaviour mono) => InitializeInstance(mono, null);
        public void InitializeAttached(MonoBehaviour mono, GameObject attachedObject) => InitializeInstance(mono, attachedObject);
        protected virtual void InitializeInstance(MonoBehaviour mono, GameObject attachedObject)
        {
            _mono = mono;
            if (Event.IsNull)
            {
                _instance.DisposeOfSelf(FMOD.Studio.STOP_MODE.IMMEDIATE);
                return;
            }

            if (attachedObject != null) _instance = AWGameManager.AudioManager.CreateAttachedInstance(Event, attachedObject);
            else _instance = AWGameManager.AudioManager.CreateInstance(Event);
            // _updateRoutine = new SingleCoroutine(mono);
            // _updateRoutine.StartRoutine(UpdateRoutine());

            _initialized = true;
        }

        public void InitializeWithEvent(MonoBehaviour mono, EventReference eventRef, GameObject attachedObject = null)
        {
            Event = eventRef;
            InitializeInstance(mono, attachedObject);
        }

        public void SetActive(bool setActive)
        {
            if (!_initialized) return;
            if (!InstanceValid) return;
            if (setActive == _active) return;

            if (setActive)
            {
                StartInstance();
            }
            else
            {
                StopInstance();
            }
        }

        public void Play() => StartInstance();
        public void Stop() => StopInstance();

        protected virtual void StartInstance()
        {
            PlayEvent(_instance);
            _active = true;
        }

        protected virtual void StopInstance()
        {
            StopEvent(_instance);
            _active = false;
        }

        protected virtual void Update() { }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                Update();
                yield return null;
            }
        }
    }
}
