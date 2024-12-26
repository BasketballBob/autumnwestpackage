using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class DelayedAlarm : Alarm
    {
        private Alarm _delayAlarm;

        public float RemainingDelay { get { return _delayAlarm.RemainingTime; } }
        public float DelayDuration { get { return _delayAlarm.Duration; } }
        public float DelayDelta { get { return _delayAlarm.Delta; } }

        public DelayedAlarm(float delayDuration, float alarmDuration) : base(alarmDuration)
        {
            _delayAlarm = new Alarm(delayDuration);
        }

        public override void Tick(float timePassed)
        {
            if (!_delayAlarm.IsFinished())
            {
                _delayAlarm.Tick(timePassed);
                return;
            }

            base.Tick(timePassed);
        }

        public override bool IsFinished()
        {
            return base.IsFinished() && _delayAlarm.IsFinished();
        }

        public bool DelayIsFinished()
        {
            return _delayAlarm.IsFinished();
        }

        public override void Reset()
        {
            base.Reset();
            _delayAlarm.Reset();
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return $"Delayed{base.ToString(format,formatProvider)} - Delay{_delayAlarm.ToString(format, formatProvider)}";
        }
    }
}
