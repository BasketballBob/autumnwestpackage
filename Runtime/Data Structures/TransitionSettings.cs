using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TransitionSettings
    {
        public Action OnLoad;
        public float EnterDuration = .5f;
        public float DelayDuration = .2f;
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
