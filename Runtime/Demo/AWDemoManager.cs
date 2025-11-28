using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AWP
{
    public class AWDemoManager : MonoBehaviour
    {
        /// <summary>
        /// Key required to hold to access these commands
        /// </summary>
        [SerializeField]
        private InputActionReference _devKey;
        [SerializeField]
        private InputActionReference _resetKey;
        [SerializeField]
        private InputActionReference _attractKey;

        [Header("Attract Mode")]
        [SerializeField]
        private VideoClip _videoClip;
        [SerializeField]
        private EventReference _videoAudio;

        private DemoAttractPlayer _demoAttractPlayer;
        private SingleCoroutine _demoRoutine;
        private Alarm _attractBeginAlarm;
        private Alarm _attractExitAlarm;
        private bool _inAttractMode;
        private bool _active;

        protected virtual float DeltaTime => Time.unscaledDeltaTime;
        protected virtual bool AttractModeEnabled => true;
        protected virtual float AttractBeginDelay => 35;
        protected virtual float AttractExitInputDuration => 1;
        protected virtual string AttractScene => "DemoAttractMode";

        protected virtual void Awake()
        {
            if (!AWGameManager.BuildTypeIsDemo()) Destroy(gameObject);
        }

        protected virtual void OnEnable()
        {
            _devKey.action.Enable();
            _resetKey.action.Enable();
            _attractKey.action.Enable();
        }

        protected virtual void OnDisable()
        {
            _devKey.action.Disable();
            _resetKey.action.Disable();
            _attractKey.action.Disable();
        }

        protected virtual void Start()
        {
            _demoRoutine = new SingleCoroutine(this);
            _attractBeginAlarm = new Alarm(AttractBeginDelay);
            _attractExitAlarm = new Alarm(AttractExitInputDuration);

            _demoRoutine.StartRoutine(DelayActivation());
        }

        protected virtual void Update()
        {
            if (!_active) return;

            ManageAttractMode();

            if (_devKey.action.IsPressed())
            {
                if (_resetKey.action.WasPressedThisFrame()) ResetDemo();
                if (_attractKey.action.WasPressedThisFrame()) ToggleAttractMode();
            }
        }

        protected virtual void ResetDemo()
        {
            
        }

        /// <summary>
        /// Delay to activate the demo manager until everything is ready (particularly stuttering on the first frame)
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayActivation()
        {
            yield return new WaitForEndOfFrame();
            _active = true;
        }

        #region Attract mode
        private void ToggleAttractMode()
        {
            if (_inAttractMode) ExitAttractMode();
            else EnterAttractMode();
        }

        private void ManageAttractMode()
        {
            if (!AttractModeEnabled) return;

            //Debug.Log($"PLAYER IS INPUTTING {PlayerIsInputting()} {_attractBeginAlarm.RemainingTime}");

            if (_inAttractMode)
            {
                if (PlayerIsInputting())
                {
                    if (_attractExitAlarm.RunOnFinish(DeltaTime))
                    {
                        ExitAttractMode();
                        _attractExitAlarm.Reset(AttractExitInputDuration);
                    }
                }
                else _attractExitAlarm.Tick(-DeltaTime);
            }
            else
            {
                if (!PlayerIsInputting())
                {
                    //Debug.Log($"TICK ATTRACT ALARM {_attractBeginAlarm.RemainingTime} {DeltaTime} {_attractBeginAlarm.RemainingTime - DeltaTime}");

                    if (_attractBeginAlarm.RunOnFinish(DeltaTime))
                    {
                        Debug.Log("ENTER ATTRACT MODE!");
                        EnterAttractMode();
                        _attractBeginAlarm.Reset(AttractBeginDelay);
                    }
                }
                else _attractBeginAlarm.Reset(AttractBeginDelay);
            }  
        }

        private void EnterAttractMode()
        {
            _demoRoutine.StartRoutine(EnterRoutine());
            _inAttractMode = true;

            IEnumerator EnterRoutine()
            {
                AudioManager.CleanUp();

                yield return AWGameManager.LoadSceneAsync(AttractScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
                while (DemoAttractPlayer.Current == null) yield return null;
                _demoAttractPlayer = DemoAttractPlayer.Current;
                
                yield return AttractRoutine();
            }
        }

        private IEnumerator AttractRoutine()
        {
            yield return _demoAttractPlayer.PlayVideoLooped(_videoClip, _videoAudio);
        }

        private void ExitAttractMode()
        {
            _demoRoutine.StopRoutine();
            AudioManager.CleanUp();
            _inAttractMode = false;

            ResetDemo();
        }

        protected virtual bool PlayerIsInputting()
        {
            return true; // WILL DO NOTHING CURRENTLY
        }
        #endregion
    }
}
