using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.BaseGameManager)]
    public abstract class BaseGameManager : MonoBehaviour
    {
        protected virtual string CameraRefName => "MainCamera";
        protected virtual string MenuManagerRefName => "EventSystem";

        public static Camera Camera { get; private set; }
        public static MenuManager MenuManager { get; private set; }

        protected virtual bool ResetScenePressed => false;
        protected virtual bool DebugBreakPressed => false;

        #region Events
            protected virtual void Awake()
            {
                OnSceneLoaded(GetCurrentScene(), default);
            }

            protected virtual void OnEnable()
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            protected virtual void OnDisable()
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }

            protected void Update()
            {
                ManageDebugCommands();
            }

            protected virtual void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
            {
                SyncReferences();
            }

            protected virtual void SyncReferences()
            {
                Camera = GameObject.Find(CameraRefName)?.GetComponent<Camera>();
                MenuManager = GameObject.Find(MenuManagerRefName)?.GetComponent<MenuManager>();
            }
        #endregion

        public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
        public static void ResetScene() => SceneManager.LoadScene(GetCurrentScene().name);

        #region Debug
            protected virtual void ManageDebugCommands()
            {
                if (ResetScenePressed) ResetScene();
                if (DebugBreakPressed) Debug.Break();
            }
        #endregion
    }
}
