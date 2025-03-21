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

        public void Transition(string scene)
        {
            _destinationScene = scene;
            _destinationSceneLoaded = false;
            _transitionRoutine.StartRoutine(TransitionRoutine());
        }

        private IEnumerator TransitionRoutine()
        {
            if (_pauseGame) AWGameManager.SetPaused(true);
            yield return _animator.WaitForAnimationToComplete("Enter");
            AWGameManager.LoadScene(_destinationScene);
            while (!_destinationSceneLoaded) yield return null;

            yield return new WaitForSecondsRealtime(_exitDelay);
            if (_pauseGame) AWGameManager.SetPaused(false);
            yield return _animator.WaitForAnimationToComplete("Exit");
            Destroy(gameObject);
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
