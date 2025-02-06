using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AWP
{
    public abstract class SingletonReferenceObject<TReference> : ReferenceObject<TReference> where TReference : Component
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

        private TReference ProcessNewValue(TReference newValue)
        {
            if (Reference == null) return newValue;

            switch (_priorityType)
            {
                case PriorityType.Old: 
                    HandleDuplicate(newValue);
                    return Reference;
                case PriorityType.New:
                    HandleDuplicate(Reference);
                    return newValue;
            }

            throw new NotImplementedException();
        }

        private void HandleDuplicate(TReference duplicate)
        {
            switch (_duplicateAction)
            {
                case DuplicateAction.DestroyRoot:
                    Destroy(duplicate.transform.root.gameObject);
                    return;
            }

            throw new NotImplementedException();
        }
    }
}
