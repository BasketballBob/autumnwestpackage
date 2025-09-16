using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [System.Serializable]
    public abstract class AWPreferenceData : SaveableData
    {
        public AWPreferenceData() { }

        public override void Save() { }

        public override void Load() { }
    }
}
