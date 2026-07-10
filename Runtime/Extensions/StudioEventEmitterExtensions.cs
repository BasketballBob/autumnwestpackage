using FMODUnity;
using UnityEngine;

namespace AWP
{
    public static class StudioEventEmitterExtensions
    {
        /// <summary>
        /// Switches out emitter event for another (USED PRIMARILY FOR CLOWN MEAT REFACTORING BallChaser.cs)
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="eventRef"></param>
        public static void SwitchEvent(this StudioEventEmitter emitter, EventReference eventRef)
        {
            emitter.EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            emitter.Stop();
            emitter.EventReference = eventRef;
            emitter.Play();
        }
    }
}
