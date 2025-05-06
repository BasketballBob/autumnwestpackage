using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System;

namespace AWP
{
    public static class FMODExtensions
    {
        #region Event instances
            public static EventInstance EnsureInstance(this EventInstance instance, EventReference eventRef)
            {
                if (instance.isValid()) return instance;
                instance = AWGameManager.AudioManager.CreateInstance(eventRef);
                return instance;
            }

            public static EventInstance EnsureAttachedInstance(this EventInstance instance, EventReference eventRef, GameObject attachedObject)
            {
                if (instance.isValid()) return instance;
                instance = AWGameManager.AudioManager.CreateAttachedInstance(eventRef, attachedObject);
                return instance;
            }

            public static void StartIfNotPlaying(this EventInstance instance)
            {
                if (!instance.isValid()) return;
                PLAYBACK_STATE playbackState = instance.GetPlaybackState();
                if (playbackState == PLAYBACK_STATE.STARTING) return;
                if (playbackState == PLAYBACK_STATE.PLAYING) return;
                if (playbackState == PLAYBACK_STATE.SUSTAINING) return;
                instance.start();
            }

            public static void StopIfPlaying(this EventInstance instance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
            {
                if (!instance.isValid()) return;
                PLAYBACK_STATE playbackState = instance.GetPlaybackState();
                if (playbackState == PLAYBACK_STATE.STOPPED) return;
                if (playbackState == PLAYBACK_STATE.STOPPING) return;
                instance.stop(stopMode);
            }

            public static IEnumerator FadeToVolume(this EventInstance instance, float duration, float endVolume, AWDelta.DeltaType deltaType = AWDelta.DeltaType.UnscaledUpdate)
            {
                float startVolume;
                instance.getVolume(out startVolume);

                yield return AnimationFX.DeltaRoutine(x => 
                {
                    instance.setVolume(startVolume.Lerp(endVolume, x));
                }, duration, EasingFunction.Sin, deltaType);
            }

            public static PLAYBACK_STATE GetPlaybackState(this EventInstance instance)
            {
                PLAYBACK_STATE returnState;
                instance.getPlaybackState(out returnState);
                return returnState;
            }
        #endregion

        public static FMOD.VECTOR ConvertToFMOD(this Vector3 vector)
        {
            FMOD.VECTOR fmodVector;
            fmodVector.x = vector.x;
            fmodVector.y = vector.y;
            fmodVector.z = vector.z;

            return fmodVector;
        }

        public static Vector3 ConvertToUnity(this FMOD.VECTOR fmodVector)
        {
            return new Vector3(fmodVector.x, fmodVector.y, fmodVector.z);
        }

        public static void SetInstancePosition(this EventInstance instance, Vector3 position)
        {
            Modify3DAttributes(instance, x => x.position = position.ToFMODVector());
        }

        public static void Modify3DAttributes(this EventInstance instance, Action<FMOD.ATTRIBUTES_3D> modifyFunc)
        {
            FMOD.ATTRIBUTES_3D prevAttributes;
            instance.get3DAttributes(out prevAttributes);
            modifyFunc(prevAttributes);
            instance.set3DAttributes(prevAttributes);
        }
    }
}
