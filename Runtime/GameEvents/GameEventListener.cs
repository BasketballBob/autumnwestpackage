using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(Response.Invoke);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(Response.Invoke);
        }

        public void OnEventRaised()
        {
            Response?.Invoke();
        }

        /// <summary>
        /// Calls the attached UnityEvent
        /// </summary>
        public void CallResponse()
        {
            Response.Invoke();
        }
    }
}
