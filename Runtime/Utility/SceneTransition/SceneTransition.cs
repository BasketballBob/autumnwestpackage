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
        private Canvas _canvas;
        [SerializeField]
        private Animator _animator;

        private SingleCoroutine _transitionRoutine;

        public Canvas Canvas => _canvas;

        private void Awake()
        {
            _transitionRoutine = new SingleCoroutine(this);
        }

        private void Start()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        public void Transition(string scene, TransitionSettings settings, SceneAudio audio = null)
        {
            if (audio == null) AWGameManager.AudioManager.GetSceneAudio(scene);
            _transitionRoutine.StartRoutine(TransitionRoutine(LoadSceneRoutine(), settings, audio));

            IEnumerator LoadSceneRoutine()
            {
                // AWGameManager.LoadScene(_destinationScene);
                // while (!_destinationSceneLoaded) yield return null;

                yield return AWGameManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            }
        }

        /// <summary>
        /// Plays transition effect, but allows you to replace scene loading with any routine
        /// </summary>
        /// <param name="transitionRoutine"></param>
        public IEnumerator CustomTransition(IEnumerator transitionRoutine, TransitionSettings settings = null, SceneAudio audio = null)
        {
            yield return _transitionRoutine.StartRoutine(TransitionRoutine(transitionRoutine, settings, audio));
        }

        private IEnumerator TransitionRoutine(IEnumerator transitionRoutine, TransitionSettings settings, SceneAudio audio)
        {
            if (settings != null && settings.OverrideSortingOrder != null) Canvas.sortingOrder = (int)settings.OverrideSortingOrder;

            if (settings == null || settings.PauseGame) AWGameManager.SetPaused(true);
            PrepareSceneAudioTransition();
            yield return EnterRoutine(settings);
            yield return transitionRoutine;

            settings.OnLoad.Invoke();

            yield return new WaitForSecondsRealtime(settings.DelayDuration);
            if (settings == null || settings.PauseGame) AWGameManager.SetPaused(false);
            yield return ExitRoutine(settings);

            void PrepareSceneAudioTransition()
            {
                if (audio == null) return;
                AWGameManager.AudioManager.EnterNewSceneAudio(audio);
            }
        }

        public IEnumerator EnterRoutine(TransitionSettings settings)
        {
            _animator.Play("Enter");
            if (settings != null) _animator.SetSpeedForDuration(settings.EnterDuration);
            yield return _animator.WaitForAnimationToComplete();
        }

        public IEnumerator ExitRoutine(TransitionSettings settings)
        {
            _animator.Play("Exit");
            if (settings != null) _animator.SetSpeedForDuration(settings.ExitDuration);
            yield return _animator.WaitForAnimationToComplete();
            Destroy(gameObject);
        }
    }
}
