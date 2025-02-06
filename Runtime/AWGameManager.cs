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
        protected virtual string CullingBoundsRefName => "CullingBounds";

        public static AWGameManager Current { get; private set; }
        public static Camera Camera { get; private set; }
        public static AWCamera AWCamera { get; private set; }
        public static MenuManager MenuManager { get; private set; }
        public static AudioManager AudioManager { get; private set; }
        public static CullingBounds CullingBounds { get; private set; }
        public static float TimeScale { get { return Time.timeScale; } set { Time.timeScale = value; }}
        public static bool IsPaused { get; protected set; }

        [SerializeField]
        private AudioManager _audioManager;

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
                Current = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            protected virtual void OnDisable()
            {
                if (Current == this) Current = null;
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
                if (Camera == null) Camera = GameObject.Find(CameraRefName)?.GetComponent<Camera>();
                if (AWCamera == null) AWCamera = GameObject.Find(CameraRefName)?.GetComponent<AWCamera>();
                if (MenuManager == null) MenuManager = GameObject.Find(MenuManagerRefName)?.GetComponent<MenuManager>();
                if (AudioManager == null) AudioManager = _audioManager;
                if (CullingBounds == null) CullingBounds = GameObject.Find(CullingBoundsRefName)?.GetComponent<CullingBounds>();
            }
        #endregion

        #region Scene Management
            public static void LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
            {
                SceneManager.LoadScene(sceneName, loadMode);
            }
            public static void LoadSceneAdditive(string sceneName)
            {
                LoadScene(sceneName, LoadSceneMode.Additive);
            }
            public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
            public static void ResetScene() => LoadScene(GetCurrentScene().name);
        #endregion

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
