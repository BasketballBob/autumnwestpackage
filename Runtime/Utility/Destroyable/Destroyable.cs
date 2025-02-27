using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    public abstract class Destroyable : MonoBehaviour, IDestroyable
    {
        [SerializeField]
        private float _destroyDuration = 1;
        [SerializeField]
        private EasingFunction _easingMode;
        [SerializeField]
        private AWDelta.DeltaType _deltaType;
        [SerializeField]
        private float _startDelay = 0;
        [SerializeField]
        private bool _immediatelyDestroy;
        [SerializeField] 
        private List<DestroyEvent> _destroyEvents;

        private bool _setToDestroy;

        protected virtual void OnEnable()
        {
            if (_immediatelyDestroy) Destroy();
        }

        protected virtual void OnDisable()
        {
            _setToDestroy = false;
        }

        public void Destroy()
        {
            if (_setToDestroy) return;

            StartCoroutine(DestroyRoutine());
            _setToDestroy = true;
        }

        protected abstract void DeltaAction(float delta);

        private IEnumerator DestroyRoutine()
        {
            yield return AWDelta.WaitForSeconds(_deltaType, _startDelay);

            LaunchDestroyEvents();
            yield return AnimationFX.DeltaRoutine(DeltaAction, _destroyDuration, _easingMode, _deltaType);

            Destroy(gameObject);
        }

        public static void Destroy(GameObject gameObject)
        {
            IDestroyable destroyable = gameObject.GetComponent<IDestroyable>();
            if (destroyable != null) destroyable.Destroy();
            else GameObject.Destroy(gameObject);
        }

        private void LaunchDestroyEvents()
        {
            _destroyEvents.ForEach(x => ActivateDestroyEvent(x));
        }

        private void ActivateDestroyEvent(DestroyEvent destroyEvent)
        {
            StartCoroutine(DestroyEventRoutine());

            IEnumerator DestroyEventRoutine()
            {
                yield return AWDelta.WaitForSeconds(_deltaType, destroyEvent.Time);
                destroyEvent.Event?.Invoke();
            }
        }   

        [System.Serializable]
        private struct DestroyEvent
        {
            public float Time;
            public UnityEvent Event;
        }
    }
}
