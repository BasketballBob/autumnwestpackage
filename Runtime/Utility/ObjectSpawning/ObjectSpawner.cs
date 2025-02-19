using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public abstract class ObjectSpawner<TObjectType> : MonoBehaviour where TObjectType : Component
    {
        [SerializeField]
        private SpawnerType _spawnerType;
        [SerializeField] [VerticalGroup("Selector")]
        protected ItemSelector<TObjectType> _objectSelector;
        [SerializeField] [VerticalGroup("Position")]
        protected PointArea _pointArea;
        [SerializeField] [VerticalGroup("Spawn")]
        private int _spawnLimit = 5;
        [SerializeField] [VerticalGroup("Spawn")]
        private bool _spawnParented;
        [SerializeField] [VerticalGroup("Delay")]
        protected float _spawnDelay = .1f;
        [SerializeField] [VerticalGroup("Routine")] [ShowIf("CanStartRoutineImmediately")]
        private bool _startRoutineImmediately;

        private ObjectLimit<TObjectType> _objectLimit;
        protected Alarm _delayAlarm;
        private Coroutine _spawningRoutine;
        private bool _spawningOverridden;

        public enum SpawnerType { Default, Oneshot };

        public ObjectLimit<TObjectType> ObjectLimit => _objectLimit;
        public bool CanSpawn => _delayAlarm.IsFinished();
        public int CurrentObjectCount => _objectLimit.CurrentCount;
        public bool SpawnRoutineActive => _spawningRoutine != null;
        protected virtual float DelayAlarmSpeed => 1;
        public virtual AWDelta.DeltaType DeltaType => AWDelta.DeltaType.FixedUpdate;
        protected Transform SpawnParent => _spawnParented ? transform : null;
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
            TObjectType newObject = ForceSpawn(modificationAction);
            StartCoroutine(_delayAlarm.RunUntilFinishRoutine(DeltaType));
        }
        /// <summary>
        /// Spawns regardless of conditions of object spawners
        /// For internal use with overriding spawning
        /// </summary>
        /// <returns></returns>
        protected TObjectType ForceSpawn(Action<TObjectType> modificationAction)
        {
            TObjectType newObject = CreateObject();
            _objectLimit.AddItem(newObject);
            modificationAction?.Invoke(newObject);
            return newObject;
        }

        public void SetSpawningActive(bool active)
        {
            if (_spawningOverridden) return;
            if (SpawnRoutineActive == active) return;

            if (active)
            {
                switch (_spawnerType) 
                {
                    case SpawnerType.Default:
                        StartSpawningRoutine(DefaultSpawningRoutine());
                        break;
                    case SpawnerType.Oneshot:
                        StartSpawningRoutine(OneShotSpawningRoutine());
                        break;
                }
            }
            else
            {
                StopSpawningRoutine();
            }
        }

        #region Spawning routines
            protected virtual IEnumerator DefaultSpawningRoutine()
            {
                while (true)
                {
                    Spawn();
                    yield return DeltaType.YieldNull();
                }
            }

            protected virtual IEnumerator OneShotSpawningRoutine()
            {
                yield return SpawnForCount(_spawnLimit, _spawnDelay, null);
            }
        #endregion

        #region Spawning helper functions
            protected IEnumerator SpawnForCount(int count, float delay, Action<TObjectType> modificationAction)
            {
                int remainingSpawn = count;

                while (remainingSpawn > 0)
                {
                    ForceSpawn(modificationAction);
                    remainingSpawn--;
                    yield return DeltaType.WaitForSeconds(delay);
                }
            }
        #endregion

        protected void StartSpawningRoutine(IEnumerator routine)
        {
            StopSpawningRoutine();
            _spawningRoutine = StartCoroutine(routine);
        }
        protected void StopSpawningRoutine()
        {
            if (_spawningRoutine != null) StopCoroutine(_spawningRoutine);
            _spawningOverridden = false;
            _spawningRoutine = null;
        }

        protected void StartSpawningOverride(IEnumerator routine)
        {
            StopSpawningRoutine();
            _spawningOverridden = true;
            _spawningRoutine = StartCoroutine(OverrideRoutine());

            IEnumerator OverrideRoutine()
            {
                yield return routine;
                _spawningOverridden = false;
            }   
        }

        public void OverrideSpawnForCount(int count, float delay, Action<TObjectType> modificationAction = null)
        {
            StartSpawningOverride(SpawnForCount(count, delay, modificationAction));
        }

        protected abstract TObjectType CreateObject();
    }
}
