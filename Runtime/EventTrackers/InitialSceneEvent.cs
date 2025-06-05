using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class InitialSceneEvent : MonoBehaviour
    {
        private static bool IsInitialScene = true;

        [SerializeField]
        private UnityEvent _onInitialScene;
        [SerializeField]
        private UnityEvent _onElse;

        private void Start()
        {
            if (IsInitialScene)
            {
                _onInitialScene?.Invoke();
                IsInitialScene = false;
            }
            else
            {
                _onElse?.Invoke();
            }
        }
    }
}
