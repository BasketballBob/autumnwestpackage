using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public class InitialSceneEvent : MonoBehaviour
    {
        private const float EventDelay = .25f;
        private static bool IsInitialScene = true;

        [Header("THIS EVENT IS DELAYED BY .1f SECONDS")]
        [SerializeField]
        private UnityEvent _onInitialScene;

        private void Awake()
        {
            if (IsInitialScene)
            {
                StartCoroutine(DelayedEvent());
                IEnumerator DelayedEvent()
                {   
                    yield return new WaitForSeconds(EventDelay);
                    _onInitialScene?.Invoke();
                    IsInitialScene = false;
                }
            }
        }
    }
}
