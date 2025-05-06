using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace AWP
{
    public abstract class AWEventParameter
    {
        public string ParameterName;
        // ONLY HANDLES GLOBAL PARAMS RIGHT NOW
        public abstract void Apply();
    }

    public abstract class AWEventParameter<TVariable> : AWEventParameter
    {
        public TVariable Value;
    }

    [System.Serializable]
    public sealed class AWFloatParameter : AWEventParameter<float>
    {
        public override void Apply()
        {
            AudioManager.SetGlobalParameterByName(ParameterName, Value);
        }
    }

    [System.Serializable]
    public sealed class AWLabeledParameter : AWEventParameter<string>
    {
        public override void Apply()
        {
            AudioManager.SetGlobalLabeledParameterByName(ParameterName, Value);
        }
    }
}
