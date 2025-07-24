using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace AWP
{
    public static class EventInstanceExtensions
    {
        /// <summary>
        /// Stops and releases the instance
        /// </summary>
        /// <param name="instance"></param>
        public static void DisposeOfSelf(this EventInstance instance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
        {
            instance.stop(stopMode);
            instance.release();
        }
    }
}
