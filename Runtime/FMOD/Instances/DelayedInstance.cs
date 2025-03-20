using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    [System.Serializable] [InlineProperty]
    public class DelayedInstance : AWEventInstance
    {
        public float DelayTime = .2f;
        private Alarm _delayAlarm;

        protected override void InitializeInstance(MonoBehaviour mono, GameObject attachedObject)
        {
            _delayAlarm = new Alarm(DelayTime, 0);
            base.InitializeInstance(mono, attachedObject);
        }

        protected override void StartInstance()
        {
            if (!_delayAlarm.IsFinished()) return;
            base.StartInstance();
            _delayAlarm.Reset();
        }

        protected override void StopInstance()
        {
            base.StopInstance();
        }

        protected override void Update()
        {
            _delayAlarm.Tick(Time.deltaTime);
        }
    }
}
