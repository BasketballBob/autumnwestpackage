using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class ObjectSpawner<TObjectType> : MonoBehaviour where TObjectType : Component
    {
        [SerializeField]
        protected ItemSelector<TObjectType> _objectSelector;
        [SerializeField]
        private int _spawnLimit = 5;
        [SerializeField]
        private float _spawnDelay;

        private ObjectLimit<TObjectType> _objectLimit;
        private Alarm _delayAlarm;

        public bool CanSpawn => _delayAlarm.IsFinished();

        protected virtual void Start()
        {
            _objectLimit = new ObjectLimit<TObjectType>(_spawnLimit);
            _delayAlarm = new Alarm(_spawnDelay, 0);
        }

        public void Spawn() => Spawn(null);
        public void Spawn(Action<TObjectType> modificationAction)
        {
            if (!CanSpawn) return;
            TObjectType newObject = CreateObject();
            _objectLimit.AddItem(newObject);
            modificationAction?.Invoke(newObject);
            StartCoroutine(_delayAlarm.RunUntilFinishRoutine(AWDelta.DeltaType.FixedUpdate));
        }

        protected abstract TObjectType CreateObject();
    }
}
