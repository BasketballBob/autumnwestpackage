using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public interface ISaveable<TData> : ISaveable
    {
        public TData GetSaveData();
        public void LoadSaveData(TData loadData);
    }

    public interface ISaveable { }
}
