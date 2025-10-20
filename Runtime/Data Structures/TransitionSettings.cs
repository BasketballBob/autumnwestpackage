using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TransitionSettings
    {
        public float EnterDuration;
        public float DelayDuration;
        public float ExitDuration;
        public bool PauseGame = true;
        public int? OverrideSortingOrder;

        public TransitionSettings() { }

        public TransitionSettings(float enterDuration, float delayDuration, float exitDuration, bool pauseGame = true)
        {
            EnterDuration = enterDuration;
            DelayDuration = delayDuration;
            ExitDuration = exitDuration;
            PauseGame = pauseGame;
        }
    }
}
