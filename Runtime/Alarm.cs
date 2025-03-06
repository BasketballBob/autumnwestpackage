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

        public void ModifyDuration(float newDuration)
        {
            float difference = newDuration - Duration;
            _duration = newDuration;
            if (difference < 0) Tick(-difference);
        }

        public void SetRemainingTime(float newTime)
        {
            _remainingTime = Mathf.Clamp(newTime, 0, Duration);
        }

        /// <summary>
        /// Deducts alarm by an amount of time
        /// </summary>
        /// <param name="timePassed">Amount of time to deduct</param>
        public virtual void Tick(float timePassed)
        {
            _remainingTime -= timePassed;
            if (_remainingTime < 0) _remainingTime = 0;
            if (_remainingTime > Duration) _remainingTime = Duration;
        }

        /// <summary>
        /// Ticks the time at a speed relative to the given duration
        /// </summary>
        /// <param name="timePassed"></param>
        /// <param name="duration"></param>
        public virtual void TickForDuration(float timePassed, float duration)
        {
            float durationRatio = Duration / duration;
            Tick(timePassed * durationRatio);
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
        /// If remainingTime is equal to duration
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFullyUnfinished()
        {
            return _remainingTime >= Duration;
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

        /// <summary>
        /// Runs once on finish of timer
        /// </summary>
        /// <param name="timePassed"></param>
        /// <returns></returns>
        public bool RunOnFinish(float timePassed)
        {
            if (IsFinished()) return false;

            Tick(timePassed);

            if (IsFinished())
            {
                return true;
            }
            else return false;
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

        public override string ToString()
        {
            return $"Alarm {_remainingTime.ToString()}/{_duration.ToString()}";
        }
    }
}