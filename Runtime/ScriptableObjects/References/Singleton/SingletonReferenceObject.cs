using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AWP
{
    public abstract class SingletonReferenceObject<TReference> : ReferenceObject<TReference>, ITransformReference where TReference : Component
    {
        [Header("Singleton handling")]
        [SerializeField] [Tooltip("Determines which singleton is prioritized if multiple exist")]
        private PriorityType _priorityType;
        [SerializeField] [Tooltip("How to handle duplicate of singleton if one exists")]
        private DuplicateAction _duplicateAction;

        private enum PriorityType { Old, New };
        private enum DuplicateAction { DestroyRoot };

        public override TReference Reference 
        { 
            get => base.Reference; 
            set
            {
                base.Reference = ProcessNewValue(value);
            } 
        }
        public Transform Transform => Reference.transform;

        private TReference ProcessNewValue(TReference newValue)
        {
            if (Reference == null) return newValue;

            switch (_priorityType)
            {
                case PriorityType.Old: 
                    HandleDuplicate(newValue);
                    HandleCurrentValue(Reference);
                    return Reference;
                case PriorityType.New:
                    HandleDuplicate(Reference);
                    HandleCurrentValue(newValue);
                    return newValue;
            }

            throw new NotImplementedException();
        }

        private void HandleDuplicate(TReference duplicate)
        {
            if (duplicate == null) return;

            switch (_duplicateAction)
            {
                case DuplicateAction.DestroyRoot:
                    Destroy(duplicate.transform.gameObject);
                    return;
            }

            throw new NotImplementedException();
        }

        private void HandleCurrentValue(TReference current)
        {
            if (current == null) return;

            current.transform.SetParent(null);
            DontDestroyOnLoad(current.gameObject);
        }
    }
}
