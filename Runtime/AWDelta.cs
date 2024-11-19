using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWDelta
    {
        public enum DeltaType { Update, FixedUpdate, UnscaledUpdate, UnscaledFixedUpdate };

        public static System.Exception DeltaTypeUnaccountedException => new System.Exception("AWDelta: DeltaType unaccounted for!");

        public static float GetDelta(this DeltaType deltaType)
        {
            switch (deltaType)
            {
                case DeltaType.Update:
                    return Time.deltaTime;
                case DeltaType.FixedUpdate:
                    return Time.fixedDeltaTime;
                case DeltaType.UnscaledUpdate:
                    return Time.unscaledDeltaTime;
                case DeltaType.UnscaledFixedUpdate:
                    return Time.fixedUnscaledDeltaTime;
            }

            throw DeltaTypeUnaccountedException;
        }

        public static IEnumerator WaitForSeconds(this DeltaType deltaType, float duration)
        {
            switch (deltaType)
            {
                case DeltaType.Update:

                    return Update_WaitForSeconds();

                    IEnumerator Update_WaitForSeconds()
                    {
                        yield return new WaitForSeconds(duration);
                    }

                case DeltaType.FixedUpdate:

                    return deltaType.FixedUpdateWait(duration);

                case DeltaType.UnscaledUpdate:

                    return UnscaledUpdate_WaitForSeconds();

                    IEnumerator UnscaledUpdate_WaitForSeconds()
                    {
                        yield return new WaitForSecondsRealtime(duration);
                    }

                case DeltaType.UnscaledFixedUpdate:

                    return deltaType.FixedUpdateWait(duration);
            }

            throw DeltaTypeUnaccountedException;
        }

        public static IEnumerator YieldNull(this DeltaType deltaType)
        {
            switch (deltaType)
            {
                case DeltaType.Update:
                    return DefaultWait();
                case DeltaType.FixedUpdate:
                    return FixedWait();
                case DeltaType.UnscaledUpdate:
                    return DefaultWait();
                case DeltaType.UnscaledFixedUpdate:
                    return FixedWait();
            }

            throw DeltaTypeUnaccountedException;

            IEnumerator DefaultWait()
            {
                yield return null;
            }

            IEnumerator FixedWait()
            {
                yield return new WaitForFixedUpdate();
            }
        }

        private static IEnumerator FixedUpdateWait(this DeltaType deltaType, float duration)
        {
            Alarm fixedAlarm = new Alarm(duration);

            while (fixedAlarm.RunWhileUnfinished(deltaType.GetDelta()))
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
