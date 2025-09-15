using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Codice.Client.Common.GameUI.Checkin;

namespace AWP
{
    /// <summary>
    /// Used https://www.youtube.com/watch?v=aUi9aijvpgs for reference
    /// </summary>
    public class FileDataHandler<TData> where TData : AWSaveData
    {
        private string _fileName;
        private string _savePath;

        // use Path.Combine to account for different OS's having different path separators
        private string FullPath => GetFullPath(_fileName, _savePath);

        public FileDataHandler(string fileName, string savePath = "")
        {
            _fileName = fileName;
            _savePath = savePath;
        }

        public TData Load()
        {
            TData loadedData = null;
            if (File.Exists(FullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(FullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }

                        loadedData = JsonUtility.FromJson<TData>(dataToLoad);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load file {e}");
                }
            }
            return loadedData;
        }

        public void Save(AWSaveData data)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
                string dataToStore = JsonUtility.ToJson(data, true);

                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save file {e}");
            }
        }

        public void Delete()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                }
                else
                {
                    Debug.LogWarning($"Tried to delete save data, but it was not found at path: {FullPath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save data {e}");
            }
        }

        public static string GetFullPath(string fileName, string savePath = "")
        {
            return Path.Combine(Application.persistentDataPath, Path.Combine(savePath, fileName));
        }
    }
}
