using System;
using System.Collections;
using System.Collections.Generic;
using AWP;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
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

        public virtual Vector2 MousePosition => default;
        public virtual UnityEngine.InputSystem.InputAction Mouse1 => default;
        public bool Mouse1Pressed => Mouse1.WasPerformedThisFrame();
        public bool Mouse1Held => Mouse1.IsPressed();
        public virtual bool Debug1Pressed => false;
        public virtual bool Debug2Pressed => false;

        protected virtual bool ResetScenePressed => false;
        protected virtual bool DebugBreakPressed => false;
        protected virtual Action OnDebug1 => null;
        protected virtual Action OnDebug2 => null;

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

            public static void SetActiveScene(Scene scene) => SceneManager.SetActiveScene(scene);
            public static void SetActiveScene(string scene) => SetActiveScene(SceneManager.GetSceneByName(scene));

            /// <summary>
            /// Checks if the scene is currently under the hierarchy (but not necessarily loaded)
            /// </summary>
            /// <param name="scene"></param>
            /// <returns></returns>
            public static bool SceneIsInHierarchy(string scene)
            {
                Scene waitScene = SceneManager.GetSceneByName(scene);
                if (!waitScene.IsValid()) return false;

                return true;
            }

            /// <summary>
            /// Checks if the scene isLoaded in the hierarchy
            /// </summary>
            /// <param name="scene"></param>
            /// <returns></returns>
            public static bool SceneIsLoaded(string scene)
            {
                Scene waitScene = SceneManager.GetSceneByName(scene);
                if (!waitScene.IsValid()) return false;
                if (!waitScene.isLoaded) return false;

                return true;
            }

            public static IEnumerator WaitForSceneToBeLoaded(string scene)
            {
                while (true)
                {
                    if (SceneIsLoaded(scene)) break;
                    yield return null;
                }
            }
        #endregion

        #region Scene transition
            public static void TransitionScene(string scene) => TransitionScene(scene, Current._defaultSceneTransition);
            public static void TransitionScene(string scene, SceneTransition transition)
            {
                SceneTransition instance = Instantiate(transition);
                instance.Transition(scene);
            }

            public static void TransitionCustom(IEnumerator customRoutine) => TransitionCustom(customRoutine, Current._defaultSceneTransition);
            public static void TransitionCustom(IEnumerator customRoutine, SceneTransition transition)
            {
                SceneTransition instance = Instantiate(transition);
                instance.CustomTransition(customRoutine);
            }
        #endregion

        public static void SetPaused(bool paused)
        {
            if (!IsPaused && paused) _prePauseTimeScale = TimeScale;

            TimeScale = paused ? 0 : _prePauseTimeScale;
            IsPaused = paused;
        }

        #region Developer mode
            public static bool IsDemoMode()
            {
                return (int)DevMode >= (int)DeveloperMode.TestBuild;
            }

            public static bool IsDeveloperMode()
            {
                return (int)DevMode >= (int)DeveloperMode.Developer;
            }
        #endregion

        #region Debug
            protected virtual void ManageDebugCommands()
            {
                if (ResetScenePressed) ResetScene();
                if (DebugBreakPressed) Debug.Break();
                if (Debug1Pressed) OnDebug1?.Invoke();
                if (Debug2Pressed) OnDebug2?.Invoke();
            }
        #endregion
    }
}
