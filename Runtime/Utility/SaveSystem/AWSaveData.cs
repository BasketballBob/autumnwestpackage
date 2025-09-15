using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public abstract class AWSaveData
    {
        public AWVariableStorageSave VariableStorageData;

        public AWSaveData() { }

        public virtual void Save()
        {
            VariableStorageData = AWVariableStorage.Data.GetSaveData();
        }

        public virtual void Load()
        {
            AWVariableStorage.Data.LoadSaveData(VariableStorageData);
        }
    }
}
