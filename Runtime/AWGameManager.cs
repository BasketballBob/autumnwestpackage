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
        public const float NullFloat = -1;

        protected virtual string CameraRefName => "MainCamera";
        protected virtual string MenuManagerRefName => "EventSystem";
        protected virtual string CullingBoundsRefName => "CullingBounds";

        public static DeveloperMode DevMode { get { return Current._devMode; } }
        protected virtual DeveloperMode _devMode => DeveloperMode.Developer;
        public static BuildType BuildType { get { return Current._buildType; } }
        protected virtual BuildType _buildType => BuildType.Default;
        public static AWGameManager Current { get; private set; }
        public static AWInputManager AWInputManager { get; private set; }
        public static AudioManager AudioManager { get; private set; }
        public static AWSaveManager SaveManager { get; private set; }
        public static AWCamera AWCamera => Current?._refAWCamera.Reference;
        public static Camera Camera => AWCamera.Camera;
        public static MenuManager MenuManager => Current?._refMenuManager.Reference;
        public static CullingBounds CullingBounds => Current?._refCullingBounds.Reference;
        public static float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
            set
            {
                _timeScale = value;
                SyncTimeScale();
            }
        }
        private static float _timeScale = 1;
        public static bool IsPaused { get; protected set; }

        [Header("Reference Objects")]
        [SerializeField]
        private ReferenceObject<AWCamera> _refAWCamera;
        [SerializeField]
        private ReferenceObject<MenuManager> _refMenuManager;
        [SerializeField]
        private ReferenceObject<CullingBounds> _refCullingBounds;
        [SerializeField]
        private AWInputManager _inputManager;
        [SerializeField]
        private AudioManager _audioManager;
        [SerializeField]
        private AWSaveManager _saveManager;
        [SerializeField]
        private SceneTransition _defaultSceneTransition;

        private static float _prePauseTimeScale;
        private static SceneTransition _sceneTransition;

        public virtual Vector2 MousePosition => default;
        public virtual UnityEngine.InputSystem.InputAction Mouse1 => default;
        public bool Mouse1Pressed => Mouse1.WasPerformedThisFrame();
        public bool Mouse1Held => Mouse1.IsPressed();
        public virtual bool Debug1Pressed => false;
        public virtual bool Debug2Pressed => false;
        public Action<bool> OnTogglePause;

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
            AWInputManager = _inputManager;
            AudioManager = _audioManager;
            SaveManager = _saveManager;
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
            Debug.Log($"UNLOAD SCENE {sceneName}");
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
            while (!operation.isDone) yield return null;
        }

        public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
        public static void ResetScene() => LoadScene(GetCurrentScene().name);

        public static bool SetActiveScene(Scene scene) => SceneManager.SetActiveScene(scene);
        public static bool SetActiveScene(string scene) => SetActiveScene(SceneManager.GetSceneByName(scene));

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
        public static void TransitionScene(string scene, SceneTransition transition, TransitionSettings settings = null)
        {
            SetTransition(transition);
            _sceneTransition.Transition(scene, settings);
        }

        public static IEnumerator TransitionCustom(IEnumerator customRoutine, SceneTransition transition = null, TransitionSettings settings = null)
        {
            if (transition == null) transition = Current._defaultSceneTransition;
            SetTransition(transition);

            return _sceneTransition.CustomTransition(customRoutine, settings);
        }

        public static IEnumerator EnterTransition(SceneTransition transition = null, TransitionSettings settings = null)
        {
            if (transition == null) transition = Current._defaultSceneTransition;
            SetTransition(transition);
            yield return _sceneTransition.EnterRoutine(settings);
        }
        public static IEnumerator ExitTransition(TransitionSettings settings = null)
        {
            if (_sceneTransition == null) yield break;
            yield return _sceneTransition.ExitRoutine(settings);
        }

        private static void SetTransition(SceneTransition transition)
        {
            if (_sceneTransition != null) Destroy(_sceneTransition.gameObject);
            _sceneTransition = Instantiate(transition);
        }

        public static void SetTransitionSortOrder(int sortOrder)
        {
            if (_sceneTransition == null) return;
            _sceneTransition.Canvas.sortingOrder = sortOrder;
        }
        #endregion

        public static void SetPaused(bool paused)
        {
            //if (!IsPaused && paused) _prePauseTimeScale = TimeScale;

            IsPaused = paused;
            SyncTimeScale();
            Debug.Log($"PAUSED {paused} Time.timeScale={Time.timeScale} _timeScale={_timeScale}");

            Current.OnTogglePause?.Invoke(IsPaused);
        }
        private static void SyncTimeScale() => Time.timeScale = IsPaused ? 0 : _timeScale;
        
        #region Developer mode
        public static bool IsTestMode()
        {
            return (int)DevMode >= (int)DeveloperMode.TestBuild;
        }

        public static bool IsDeveloperMode()
        {
            return (int)DevMode >= (int)DeveloperMode.Developer;
        }

        public static bool IsMinimumMode(DeveloperMode mode)
        {
            return (int)DevMode >= (int)mode;
        }
        #endregion

        #region Build Types
        public static bool BuildTypeIsDemo()
        {
            return BuildType == BuildType.Demo;
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
