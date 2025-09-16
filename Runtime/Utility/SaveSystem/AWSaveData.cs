using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public abstract class AWSaveData : SaveableData
    {
        public AWVariableStorageSave VariableStorageData;

        public AWSaveData() { }

        public override void Save()
        {
            VariableStorageData = AWVariableStorage.Data.GetSaveData();
        }

        public override void Load()
        {
            AWVariableStorage.Data.LoadSaveData(VariableStorageData);
        }
    }
}
