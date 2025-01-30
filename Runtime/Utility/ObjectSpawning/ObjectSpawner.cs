using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public abstract class ObjectSpawner<TObjectType> : MonoBehaviour where TObjectType : Component
    {
        [SerializeField] [VerticalGroup("Selector")]
        protected ItemSelector<TObjectType> _objectSelector;
        [SerializeField] [VerticalGroup("Position")]
        protected PointArea _pointArea;
        [SerializeField] [VerticalGroup("Spawn")]
        private int _spawnLimit = 5;
        [SerializeField] [VerticalGroup("Delay")]
        protected float _spawnDelay = .1f;
        [SerializeField] [VerticalGroup("Routine")] [ShowIf("CanStartRoutineImmediately")]
        private bool _startRoutineImmediately;

        private ObjectLimit<TObjectType> _objectLimit;
        protected Alarm _delayAlarm;
        private Coroutine _spawningRoutine;

        public bool CanSpawn => _delayAlarm.IsFinished();
        public int CurrentObjectCount => _objectLimit.CurrentCount;
        public bool SpawnRoutineActive => _spawningRoutine != null;
        protected virtual float DelayAlarmSpeed => 1;
        public virtual AWDelta.DeltaType DeltaType => AWDelta.DeltaType.FixedUpdate;
        protected virtual bool CanStartRoutineImmediately => true;

        protected virtual void Awake()
        {
            _objectLimit = new ObjectLimit<TObjectType>(_spawnLimit);
            _delayAlarm = new Alarm(_spawnDelay, 0);
        }

        protected virtual void Start()
        {
            if (_startRoutineImmediately) SetSpawningActive(true);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            _pointArea.DrawGizmos(transform.position);
        }

        public void Spawn() => Spawn(null);
        public void Spawn(Action<TObjectType> modificationAction)
        {
            if (!CanSpawn) return;
            TObjectType newObject = CreateObject();
            _objectLimit.AddItem(newObject);
            modificationAction?.Invoke(newObject);
            StartCoroutine(_delayAlarm.RunUntilFinishRoutine(DeltaType));
        }

        public void SetSpawningActive(bool active)
        {
            if (SpawnRoutineActive == active) return;

            if (active)
            {
                StartSpawningRoutine(SpawningRoutine());
            }
            else
            {
                StopSpawningRoutine();
            }
        }

        protected virtual IEnumerator SpawningRoutine()
        {
            while (true)
            {
                Spawn();
                yield return DeltaType.YieldNull();
            }
        }

        protected void StartSpawningRoutine(IEnumerator routine)
        {
            StopSpawningRoutine();
            _spawningRoutine = StartCoroutine(routine);
        }
        protected void StopSpawningRoutine()
        {
            if (_spawningRoutine != null) StopCoroutine(_spawningRoutine);
            _spawningRoutine = null;
        }

        protected abstract TObjectType CreateObject();
    }
}
