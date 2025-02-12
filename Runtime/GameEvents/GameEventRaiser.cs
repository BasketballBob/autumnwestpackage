using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class GameEventRaiser : MonoBehaviour
    {
        [SerializeField]
        private List<GameEvent> _events;

        public void RaiseGameEvent(string name)
        {
            RaiseGameEvent(GetGameEvent(name));
        }

        public void RaiseGameEvent(GameEvent gameEvent)
        {
            gameEvent.Raise();
        }

        private GameEvent GetGameEvent(string name)
        {
            foreach (GameEvent element in _events)
            {
                if (element.name == name)
                {
                    return element;
                }
            }
            
            return null;
        }
    }
}
