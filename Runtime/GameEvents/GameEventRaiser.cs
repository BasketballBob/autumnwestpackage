using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class GameEventRaiser : MonoBehaviour
    {
        [SerializeField]
        private GameEvent _gameEvent;

        public void RaiseGameEvent()
        {
            _gameEvent.Raise();
        }
    }
}
