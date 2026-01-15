using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AWP
{
    /// <summary>
    /// Used to call events depending on whether this is the first loaded scene
    /// </summary>
    public class InitialSceneEvent : MonoBehaviour
    {
        private static bool IsInitialScene = true;

        /// <summary>
        /// Events are also called if the scene is loaded by itself
        /// </summary>
        [SerializeField]
        private bool _appliesToSoloLoad = true;
        [SerializeField]
        private UnityEvent _onInitialScene;
        [SerializeField]
        private UnityEvent _onElse;

        private void Awake()
        {
            if (IsInitial())
            {
                _onInitialScene?.Invoke();
                IsInitialScene = false;
            }
            else
            {
                _onElse?.Invoke();
            }
        }

        private bool IsInitial()
        {
            if (IsInitialScene) return true;
            //Debug.Log($"EEEEE {_appliesToSoloLoad} {SceneManager.loadedSceneCount}");
            if (_appliesToSoloLoad && SceneManager.loadedSceneCount == 0) return true;

            return false;
        }
    }
}
