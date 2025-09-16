using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Base class for data to be saved (like game saves or preferences)
    /// </summary>
    public abstract class SaveableData
    {
        public abstract void Save();
        public abstract void Load();
    }
}
