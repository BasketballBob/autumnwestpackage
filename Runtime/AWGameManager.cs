using System.Collections;
using System.Collections.Generic;
using AWP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AWP
{
    [DefaultExecutionOrder(AWExecutionOrder.BaseGameManager)]
    public abstract class AWGameManager : MonoBehaviour
    {
        protected virtual string CameraRefName => "MainCamera";
        protected virtual string MenuManagerRefName => "EventSystem";

        public static Camera Camera { get; private set; }
        public static Camera2D Camera2D { get; private set; }
        public static MenuManager MenuManager { get; private set; }
        public static float TimeScale { get { return Time.timeScale; } set { Time.timeScale = value; }}
        public static bool IsPaused { get; protected set; }

        private static float _prePauseTimeScale;

        protected virtual bool ResetScenePressed => false;
        protected virtual bool DebugBreakPressed => false;

        #region Events
            protected virtual void Awake()
            {
                DontDestroyOnLoad(transform.gameObject);
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
                Camera2D = GameObject.Find(CameraRefName)?.GetComponent<Camera2D>();
                MenuManager = GameObject.Find(MenuManagerRefName)?.GetComponent<MenuManager>();
            }
        #endregion

        public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
        public static void ResetScene() => SceneManager.LoadScene(GetCurrentScene().name);
        public static void SetPaused(bool paused)
        {
            if (!IsPaused && paused) _prePauseTimeScale = TimeScale;

            TimeScale = paused ? 0 : _prePauseTimeScale;
            IsPaused = paused;
        }

        #region Debug
            protected virtual void ManageDebugCommands()
            {
                if (ResetScenePressed) ResetScene();
                if (DebugBreakPressed) Debug.Break();
            }
        #endregion
    }
}
