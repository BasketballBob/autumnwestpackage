using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace AWP
{
    [System.Serializable][InlineProperty]
    public class LoopingInstance : AWEventInstance
    {
        protected override Action<EventInstance> StopEvent => _optionalStopEvent.IsNull ? base.StopEvent :
            (x) => AWGameManager.AudioManager.PlayOneShot(_optionalStopEvent);

        [SerializeField]
        private EventReference _optionalStopEvent;
    }
}
