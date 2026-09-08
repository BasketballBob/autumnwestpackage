using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public interface ISaveable<TData> : ISaveable
    {
        public TData GetSaveData();
        public void LoadSaveData(TData loadData);
        public void LoadEmptyData()
        {
            LoadSaveData((TData)Activator.CreateInstance(typeof(TData)));
            (this as ISerializationCallbackReceiver)?.OnAfterDeserialize();
        }
    }

    public interface ISaveable { }
}
