using System;
using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.OdinInspector;
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

        public static DeveloperMode DevMode { get { return _devMode; } private set {_devMode = value; } }
        private static DeveloperMode _devMode = DeveloperMode.Developer;
        public static AWGameManager Current { get; private set; }
        public static AudioManager AudioManager { get; private set; }
        public static AWCamera AWCamera => Current?._refAWCamera.Reference;
        public static MenuManager MenuManager => Current?._refMenuManager.Reference;
        public static CullingBounds CullingBounds => Current?._refCullingBounds.Reference;
        public static float TimeScale { get { return Time.timeScale; } set { Time.timeScale = value; }}
        public static bool IsPaused { get; protected set; }

        [Header("Reference Objects")]
        [SerializeField]
        private ReferenceObject<AWCamera> _refAWCamera;
        [SerializeField]
        private ReferenceObject<MenuManager> _refMenuManager;
        [SerializeField]
        private ReferenceObject<CullingBounds> _refCullingBounds;
        [SerializeField]
        private AudioManager _audioManager;
        [SerializeField]
        private SceneTransition _defaultSceneTransition;

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
                AudioManager = _audioManager;
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
                
            }

            public virtual void LoadReferences(AWSceneSingletons sceneSingletons)
            {
                _refAWCamera.Reference = sceneSingletons.AWCamera;
                _refMenuManager.Reference = sceneSingletons.MenuManager;
                _refCullingBounds.Reference = sceneSingletons.CullingBounds;
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

            /// <summary>
            /// Referenced: https://stackoverflow.com/questions/78875238/unity-how-to-call-a-function-after-a-scene-is-loaded-but-before-awake-or-onenabl
            /// .9 is evidently a magic number that even Unity uses in official documentation: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/AsyncOperation-allowSceneActivation.html
            /// </summary>
            /// <param name="sceneName"></param>
            /// <param name="loadMode"></param>
            /// <returns></returns>
            public static IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadMode);
                operation.allowSceneActivation = false;

                while (!operation.isDone && operation.progress < 0.9f)
                {
                    yield return null;
                }

                operation.allowSceneActivation = true;
            }
            public static IEnumerator LoadSceneAsyncAdditive(string sceneName) => LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            public static IEnumerator UnloadSceneAsync(string sceneName)
            {
                AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
                while (!operation.isDone) yield return null;
            }

            public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
            public static void ResetScene() => LoadScene(GetCurrentScene().name);
        #endregion

        #region Scene transition
            public static void TransitionScene(string scene) => TransitionScene(Current._defaultSceneTransition, scene);
            public static void TransitionScene(SceneTransition transition, string scene)
            {
                SceneTransition instance = Instantiate(transition);
                instance.Transition(scene);
            }
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
