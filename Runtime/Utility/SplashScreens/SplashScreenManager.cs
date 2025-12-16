using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AWP
{
    public class SplashScreenManager : MonoBehaviour
    {
        private const string EnterAnimation = "Enter";
        private const string ExitAnimation = "Exit";
        private static bool SplashScreenPlayed;

        [Header("References")]
        [SerializeField]
        private Animator _anim;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private RectTransform _splashScreenHolder;
        [SerializeField]
        private InputActionReference _progressInput;
        [SerializeField]
        [AssetsOnly]
        private List<SplashScreen> _splashScreenPrefabs;

        [Header("Variables")]
        [SerializeField]
        private float _splashScreenDelay = .5f;

        private void Awake()
        {
            CheckToDestroy();

            SetGameInteractable(false);
            StartCoroutine(SplashRoutine());
        }

        private IEnumerator SplashRoutine()
        {
            yield return _anim.WaitForAnimationToComplete(EnterAnimation);

            for (int i = 0; i < _splashScreenPrefabs.Count; i++)
            {
                SplashScreen splashScreen = Instantiate(_splashScreenPrefabs[i], _splashScreenHolder);
                splashScreen.Rect.pivot = new Vector2(.5f, .5f);
                splashScreen.Rect.anchorMax = new Vector2(.5f, .5f);
                splashScreen.Rect.anchorMin = new Vector2(.5f, .5f);
                splashScreen.Rect.anchoredPosition = Vector2.zero;
                splashScreen.Rect.localScale = Vector3.one;

                Coroutine displayRoutine = splashScreen.StartCoroutine(splashScreen.Display());
                while (!splashScreen.DisplayIsFinished())
                {
                    if (_progressInput.action.WasPerformedThisFrame()) break;
                    yield return null;
                }

                Destroy(splashScreen.gameObject);

                yield return new WaitForSecondsRealtime(_splashScreenDelay);
            }

            SetGameInteractable(true);
            yield return _anim.WaitForAnimationToComplete(ExitAnimation);

            SplashScreenPlayed = true;
            CheckToDestroy();
        }

        /// <summary>
        /// Used to ensure the player can't interact with the game while the splash screens are available
        /// </summary>
        /// <param name="value"></param>
        private void SetGameInteractable(bool value)
        {
            _canvasGroup.blocksRaycasts = !value;
        }

        /// <summary>
        /// Destroy if it has already run the splash screens
        /// </summary>
        private void CheckToDestroy()
        {
            // TEST
            if (AWGameManager.IsTestMode() || AWGameManager.BuildTypeIsDemo())
            {
                Destroy(gameObject);
                return;
            }

            if (!ShouldDestroy()) return;
            Destroy(gameObject);
        }
        
        private bool ShouldDestroy()
        {
            if (SplashScreenPlayed) return true;
            if (Time.time > 3) return true; // DESTROY IF NOT AT BEGINNING OF GAME (TESTING)

            return false;
        }
    }
}
