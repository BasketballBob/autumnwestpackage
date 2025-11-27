using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private DemoAttractPlayer _demoAttractPlayer;
        private SingleCoroutine _managerRoutine;
        private Alarm _attractBeginAlarm;

        protected virtual bool AttractModeEnabled => true;
        protected virtual float AttractBeginDelay => 60;
        protected virtual string AttractScene => "DemoAttractMode";

        private void Awake()
        {
            if (!AWGameManager.BuildTypeIsDemo()) Destroy(gameObject);
        }

        private void OnEnable()
        {
            _devKey.action.Enable();
            _resetKey.action.Enable();
        }

        private void OnDisable()
        {
            _devKey.action.Disable();
            _resetKey.action.Disable();
        }

        private void Start()
        {
            _attractBeginAlarm = new Alarm(AttractBeginDelay);
        }

        private void Update()
        {
            ManageAttractMode();

            if (_devKey.action.IsPressed())
            {
                if (_resetKey.action.WasPressedThisFrame()) ResetDemo();
            }
        }

        protected virtual void ResetDemo()
        {
            
        }

        #region Attract mode
        private void ManageAttractMode()
        {
            if (!AttractModeEnabled) return;

            if (!PlayerIsInputting() && _attractBeginAlarm.RunOnFinish(Time.deltaTime))
            {
                EnterAttractMode();
            }
            else _attractBeginAlarm.Reset(AttractBeginDelay);
        }

        private void EnterAttractMode()
        {


            IEnumerator EnterRoutine()
            {
                yield return AWGameManager.LoadSceneAsync(AttractScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
                while (DemoAttractPlayer.Current == null) yield return null;
                _demoAttractPlayer = DemoAttractPlayer.Current;
            }
        }

        protected virtual bool PlayerIsInputting()
        {
            return true; // WILL DO NOTHING CURRENTLY
        }
        #endregion
    }
}
