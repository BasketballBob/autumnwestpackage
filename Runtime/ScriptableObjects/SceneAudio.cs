using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class SceneAudio : AWScriptableObject
    {
        private const string CreateItemName = "SceneAudio";
        private const string ResourcesFolder = "SceneAudio/";

        public SceneAudioSettings Music;
        public SceneAudioSettings Ambience;
        public SceneAudioSettings Snapshot;
        public List<SRWrapper<AWEventParameter>> GlobalParams;

        public void ApplyGlobalParameters()
        {
            GlobalParams.ForEach(x => x.Value.ApplyGlobal());
        }

        [System.Serializable]
        public class SceneAudioSettings : IEquatable<SceneAudioSettings>
        {
            public EventReference EventReference;
            public float Volume = 1;

            public bool Equals(SceneAudioSettings other)
            {
                if (EventReference.Guid != other.EventReference.Guid) return false;
                if (Volume != other.Volume) return false;
                return true;
            }
        }

        public static SceneAudio LoadSceneAudio(Scene scene)
        {
            return LoadSceneAudio(scene.name);
        }

        public static SceneAudio LoadSceneAudio(string name)
        {
            return Resources.Load<SceneAudio>(ResourcesFolder + name);
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format, formatProvider);
        }
    }
}
