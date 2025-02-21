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
        public const string CreateItemName = "GameEvent";
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

        public void UnregisterListener(Action listener)
        {
            listeners.Remove(listener);
        }

        #if UNITY_EDITOR
            [Button("Test Raise")]
            private void TestRaise() => Raise();
        #endif
    }
}
