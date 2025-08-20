using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class GameEvent : ScriptableObject
    {
        private const string CreateItemName = "GameEvent";
        public const string CreateFolderName = "GameEvents/";

        private List<Action> listeners = new List<Action>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].Invoke();
            }
        }

        public void RegisterListener(Action listener)
        {
            listeners.Add(listener);
        }

        public void RegisterOneShotListener(Action listener)
        {
            Action oneShot = null;
            oneShot = () =>
            {
                listener.Invoke();
                UnregisterListener(oneShot);
            };

            RegisterListener(oneShot);
        }
        /// <summary>
        /// Adds a one shot that is only removed once it returns true
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterConditionalOneShotListener(Func<bool> listener)
        {
            Action oneShot = null;
            oneShot = () =>
            {
                if (listener.Invoke())
                {
                    UnregisterListener(oneShot);
                }
            };

            RegisterListener(oneShot);
        }

        public void UnregisterListener(Action listener)
        {
            listeners.Remove(listener);
        }

        public IEnumerator WaitOnRaise()
        {
            bool raised = false;
            RegisterOneShotListener(OnRaise);
            while (!raised) yield return null;

            void OnRaise()
            {
                raised = true;
            }   
        }

        #if UNITY_EDITOR
        [Button("Test Raise")]
            private void TestRaise() => Raise();
        #endif
    }
}
