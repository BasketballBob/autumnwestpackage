using System;
using System.Collections;
using UnityEngine;

namespace AWP
{
    public class Alarm : IFormattable
    {
        protected float _remainingTime;
        protected float _duration;

        public float RemainingTime { get { return _remainingTime; } }
        public float Duration { get { return _duration; } }
        public float Delta => 1 - Mathf.Clamp01(RemainingTime / Duration);

        public Alarm(float duration)
        {
            _duration = duration;
            _remainingTime = _duration;
        }

        public Alarm(float duration, float remainingTime)
        {
            _duration = duration;
            _remainingTime = remainingTime;
        }

        /// <summary>
        /// Deducts alarm by an amount of time
        /// </summary>
        /// <param name="timePassed">Amount of time to deduct</param>
        public virtual void Tick(float timePassed)
        {
            _remainingTime -= timePassed;
            if (_remainingTime < 0) _remainingTime = 0;
        }

        /// <summary>
        /// If alarm has finished
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFinished()
        {
            return _remainingTime <= 0;
        }

        /// <summary>
        /// Resets the RemainingTime to the Duration
        /// </summary>
        public virtual void Reset()
        {
            _remainingTime = _duration;
        }

        public bool RunWhileFinished(float timePassed)
        {
            Tick(timePassed);
            return IsFinished();
        }

        public bool RunWhileUnfinished(float timePassed)
        {
            Tick(timePassed);
            return !IsFinished();
        }

        public IEnumerator RunUntilFinishRoutine(AWDelta.DeltaType deltaType, bool reset = true)
        {
            if (reset) Reset();

            while (RunWhileUnfinished(deltaType.GetDelta()))
            {
                yield return deltaType.YieldNull();
            }
        }

        public float GetSmoothDelta(EasingFunction easingType)
        {
            return easingType.GetEasedDelta(Delta);
        }

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return $"Alarm {_remainingTime.ToString(format, formatProvider)}/{_duration.ToString(format, formatProvider)}";
        }
    }
}