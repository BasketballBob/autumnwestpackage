using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class ReferenceObject<TReference> : ReferenceObject where TReference : class
    {
        protected new const string CreateFolderName = "Reference Objects/";

        [SerializeField]
        private TReference _reference;

        public virtual TReference Reference
        {
            get { return _reference; }
            set
            {
                _reference = value;
                OnValueChanged?.Invoke();
            }
        }
        public override Component Component => _reference as Component;

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ReferenceObject : AWScriptableObject
    {
        public abstract Component Component { get; }
    }
}
