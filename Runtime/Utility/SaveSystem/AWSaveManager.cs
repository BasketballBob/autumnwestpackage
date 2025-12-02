using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;

namespace AWP
{
    public abstract class AWSaveManager<TSaveData, TPreferenceData> : AWSaveManager where TSaveData : SaveableData, new() where TPreferenceData : SaveableData, new()
    {
        private const string DefaultSaveName = "Save1.txt";
        private const string PreferencesSaveName = "Preferences.txt";

        [SerializeField]
        private SaveSettings _settings;

        [NonSerialized] [ShowInInspector]
        public TSaveData SaveData = new TSaveData();
        [NonSerialized] [ShowInInspector]
        public TPreferenceData PreferenceData = new TPreferenceData();

        private void Awake()
        {
            LoadPreferences();
        }

        [Button()]
        public void Save(string fileName = DefaultSaveName)
        {
            SaveExternalData();
            Save<TSaveData>(fileName, ref SaveData);
            Debug.Log("AWSaveManager: Save");
        }

        [Button()]
        public void Load(string fileName = DefaultSaveName)
        {
            Load<TSaveData>(fileName, ref SaveData);
            LoadExternalData();

            Debug.Log("AWSaveManager: Load");
        }

        public void LoadEmptySave(string fileName = DefaultSaveName)
        {
            LoadEmpty<TSaveData>(fileName, ref SaveData);
            Debug.Log("AWSaveManager: Load Empty");
        }

        [Button()]
        public void Delete(string fileName = DefaultSaveName) => Delete<TSaveData>(fileName);

        public bool SaveExists(string fileName = DefaultSaveName)
        {
            return File.Exists(FileDataHandler<TSaveData>.GetFullPath(fileName));
        }

        /// <summary>
        /// Saves data relevant to the SaveManager into the SaveData
        /// </summary>
        protected abstract void SaveExternalData();
        /// <summary>
        /// Loads data relevant to the SaveManager into the SaveData
        /// </summary>
        protected abstract void LoadExternalData();

        #region Preferences
        [Button()]
        public override void SavePreferences() => Save<TPreferenceData>(PreferencesSaveName, ref PreferenceData);
        [Button()]
        public override void LoadPreferences() => Load<TPreferenceData>(PreferencesSaveName, ref PreferenceData);
        #endregion

        #region Helper functions
        public void Save<TData>(string fileName, ref TData saveData) where TData : SaveableData, new()
        {
            FileDataHandler<TData> dataHandler = new FileDataHandler<TData>(fileName);

            saveData.Save();
            dataHandler.Save(saveData);
            OnSave?.Invoke();
        }

        public void Load<TData>(string fileName, ref TData saveData) where TData : SaveableData, new()
        {
            FileDataHandler<TData> dataHandler = new FileDataHandler<TData>(fileName);

            saveData = dataHandler.Load() as TData;
            if (saveData == null) saveData = new TData();

            saveData.Load();
            OnLoad?.Invoke();
        }

        public void LoadEmpty<TData>(string fileName, ref TData saveData) where TData : SaveableData, new()
        {
            saveData = new TData();
            saveData.Load();
            OnLoad?.Invoke();
        }

        public void Delete<TData>(string fileName) where TData : SaveableData
        {
            FileDataHandler<TData> dataHandler = new FileDataHandler<TData>(fileName);
            dataHandler.Delete();
        }

        public bool SaveExists<TData>(string fileName) where TData : SaveableData
        {
            return File.Exists(FileDataHandler<TData>.GetFullPath(fileName));
        }
        #endregion
    }

    public abstract class AWSaveManager : MonoBehaviour
    {
        public Action OnLoad;
        public Action OnSave;

        public abstract void SavePreferences();
        public abstract void LoadPreferences();
    }
}
