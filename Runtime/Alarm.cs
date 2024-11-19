using UnityEngine;

namespace AWP
{
    public class Alarm
    {
        private float _remainingTime;
        private float _duration;

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
        public void Tick(float timePassed)
        {
            _remainingTime -= timePassed;
            if (_remainingTime < 0) _remainingTime = 0;
        }

        /// <summary>
        /// If alarm has finished
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            return _remainingTime <= 0;
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
    }
}