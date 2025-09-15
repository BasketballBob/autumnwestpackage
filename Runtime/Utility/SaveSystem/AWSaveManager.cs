using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;

namespace AWP
{
    public abstract class AWSaveManager<TSaveData> : MonoBehaviour where TSaveData : AWSaveData, new()
    {
        private const string DefaultSaveName = "Save1.txt";

        [SerializeField]
        private SaveSettings _settings;

        public TSaveData SaveData = new TSaveData();
        private FileDataHandler<TSaveData> _dataHandler;

        [Button()]
        public void Save(string fileName = DefaultSaveName)
        {
            _dataHandler = new FileDataHandler<TSaveData>(fileName);

            SaveExternalData();
            SaveData.Save();
            _dataHandler.Save(SaveData);
        }

        [Button()]
        public void Load(string fileName = DefaultSaveName)
        {
            _dataHandler = new FileDataHandler<TSaveData>(fileName);

            SaveData = _dataHandler.Load() as TSaveData;
            if (SaveData == null) SaveData = new TSaveData();

            SaveData.Load();
            LoadExternalData();

            Debug.Log($"SAVE DATA {SaveData != null} {_dataHandler.Load() != null}");
        }

        [Button()]
        public void Delete(string fileName = DefaultSaveName)
        {
            _dataHandler = new FileDataHandler<TSaveData>(fileName);
            _dataHandler.Delete();
        }

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
    }
}
