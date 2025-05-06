using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AWP
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private bool _pauseGame = true;
        [SerializeField]
        private float _exitDelay = .2f;

        private SingleCoroutine _transitionRoutine;
        private string _destinationScene;
        private bool _destinationSceneLoaded;

        private void Awake()
        {
            _transitionRoutine = new SingleCoroutine(this);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        public void Transition(string scene, SceneAudio audio = null)
        {
            _destinationScene = scene;
            _destinationSceneLoaded = false;
            if (audio == null) AWGameManager.AudioManager.GetSceneAudio(scene);
            _transitionRoutine.StartRoutine(TransitionRoutine(LoadSceneRoutine(), audio));

            IEnumerator LoadSceneRoutine()
            {
                AWGameManager.LoadScene(_destinationScene);
                while (!_destinationSceneLoaded) yield return null;
            }
        }

        /// <summary>
        /// Plays transition effect, but allows you to replace scene loading with any routine
        /// </summary>
        /// <param name="transitionRoutine"></param>
        public IEnumerator CustomTransition(IEnumerator transitionRoutine, SceneAudio audio = null)
        {
            yield return _transitionRoutine.StartRoutine(TransitionRoutine(transitionRoutine, audio));
        }

        private IEnumerator TransitionRoutine(IEnumerator transitionRoutine, SceneAudio audio)
        {
            Action onSwitch = null;

            if (_pauseGame) AWGameManager.SetPaused(true);
            PrepareSceneAudioTransition();
            yield return EnterRoutine();
            yield return transitionRoutine;
            yield return new WaitForSecondsRealtime(_exitDelay);
            if (_pauseGame) AWGameManager.SetPaused(false);

            onSwitch?.Invoke();

            yield return ExitRoutine();
            Destroy(gameObject);

            void PrepareSceneAudioTransition()
            {
                if (audio == null) return;
                AWGameManager.AudioManager.EnterNewSceneAudio(audio, onSwitch: onSwitch);
            }
        }

        public IEnumerator EnterRoutine(float duration = AWGameManager.NullFloat)
        {
            if (duration != AWGameManager.NullFloat) _animator.SetSpeedForDuration(duration);
            _animator.Play("Enter");
            yield return _animator.WaitForAnimationToComplete();
        }

        public IEnumerator ExitRoutine(float duration = AWGameManager.NullFloat)
        {
            if (duration != AWGameManager.NullFloat) _animator.SetSpeedForDuration(duration);
            _animator.Play("Exit");
            yield return _animator.WaitForAnimationToComplete();
        }

        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            if (loadedScene.name == _destinationScene)
            {
                _destinationSceneLoaded = true;
            }
        }
    }
}
