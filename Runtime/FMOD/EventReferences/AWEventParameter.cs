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
        public abstract void ApplyGlobal();
        public abstract void ApplyLocal(EventInstance instance);

        /// <summary>
        /// Call to get the current state of the parameter as a new instance of their type
        /// </summary>
        /// <returns></returns>
        public abstract AWEventParameter GetCurrentState();
    }

    public abstract class AWEventParameter<TVariable> : AWEventParameter
    {
        public TVariable Value;

        /// <summary>
        /// Gets the current value of the parameter
        /// </summary>
        /// <returns></returns>
        public abstract TVariable GetCurrentValue();
    }

    [System.Serializable]
    public sealed class AWFloatParameter : AWEventParameter<float>
    {
        public override void ApplyGlobal()
        {
            AudioManager.SetGlobalParameterByName(ParameterName, Value);
        }

        public override void ApplyLocal(EventInstance instance)
        {
            instance.setParameterByName(ParameterName, Value);
        }

        public override AWEventParameter GetCurrentState()
        {
            AWFloatParameter current = new AWFloatParameter();
            current.ParameterName = ParameterName;
            current.Value = GetCurrentValue();

            return current;
        }

        public override float GetCurrentValue() => AudioManager.GetGlobalParameterByName(ParameterName);
    }

    [System.Serializable]
    public sealed class AWLabeledParameter : AWEventParameter<string>
    {
        public override void ApplyGlobal()
        {
            AudioManager.SetGlobalLabeledParameterByName(ParameterName, Value);
        }

        public override void ApplyLocal(EventInstance instance)
        {
            instance.setParameterByNameWithLabel(ParameterName, Value);
        }

        public override AWEventParameter GetCurrentState()
        {
            AWLabeledParameter current = new AWLabeledParameter();
            current.ParameterName = ParameterName;
            current.Value = GetCurrentValue();

            return current;
        }

        public override string GetCurrentValue() => AudioManager.GetGlobalLabeledParameterByName(ParameterName);
    }
}
