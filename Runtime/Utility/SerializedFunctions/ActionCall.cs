using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AWP
{
    public class ActionCall
    {
        [HideInInspector]
        public Component Component;
        [HideInInspector]
        public string Action;

        protected System.Reflection.MethodInfo _methodInfo;
        protected bool _initialized;

        public ActionCall(Component component, string action)
        {
            Component = component;
            Action = action;
        }

        public virtual void Invoke()
        {
            Invoke(null);
        }

        protected void Invoke(object[] parameters)
        {
            EnsureInitialized();
            _methodInfo.Invoke(Component, parameters);
        }

        public override string ToString()
        {
            return typeof(Component).Name + "/" + Action;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            
            _methodInfo = Component.GetType().GetMethod(Action);
            _initialized = true;
        }
    }

    public class ActionCall<T> : ActionCall
    {
        public ValueObject<T> Parameter1 = new ValueObject<T>();

        public ActionCall(Component component, string action) : base(component, action) { }

        public override void Invoke()
        {
            base.Invoke(new object[] { Parameter1 as object });
        }

        public override string ToString()
        {
            return base.ToString() + $"<{typeof(T).Name}>";
        }
    }
}
