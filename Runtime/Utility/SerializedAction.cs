using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    [InlineProperty]
    public class SerializedAction
    {
        [SerializeField] [HorizontalGroup("Main")] [HideLabel]
        private GameObject _gameObject;
        [OdinSerialize] [HorizontalGroup("Main")] [HideLabel] [ShowIf("@_gameObject != null")] [ValueDropdown("GetAllFunctions")] //[CustomValueDrawer("TestValueDrawer")]
        private FunctionCall _functionCall;

        private FunctionCall TestValueDrawer(FunctionCall value, GUIContent label)
        {
            EditorGUILayout.ColorField(Color.green);
            return value;
        }

        public void Invoke()
        {
            _functionCall.Invoke();
        }

        private IEnumerable GetAllFunctions()
        {
            if (_gameObject == null) return null;

            ValueDropdownList<FunctionCall> returnList = new ValueDropdownList<FunctionCall>();

            foreach (Component component in _gameObject.GetComponents<Component>())
            {
                foreach (MethodInfo methodInfo in component.GetType().GetMethods())
                {
                    string dropdownName = methodInfo.DeclaringType.Name + "/" + methodInfo.Name;
                    FunctionCall functionCall = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    if (parameters.Length > 0 && methodInfo.IsPublic)
                    {
                        try
                        {
                            Type genericType = typeof(FunctionCall<>).MakeGenericType(new Type[] { parameters[0].ParameterType });
                            functionCall = Activator.CreateInstance(genericType, new object[] { component, methodInfo.Name }) as FunctionCall;
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"TYPE CAUGHT ERROR {parameters[0].ParameterType} " + e);
                            continue;
                        }
                    }
                    else functionCall = new FunctionCall(component, methodInfo.Name);

                    returnList.Add(new ValueDropdownItem<FunctionCall>(dropdownName, functionCall));
                }
            }

            return returnList;
        }

        [System.Serializable]
        private class FunctionCall
        {
            //[HideInInspector]
            public Component Component;
            //[HideInInspector]
            public string Function;

            protected System.Reflection.MethodInfo _methodInfo;
            protected bool _initialized;

            public FunctionCall(Component component, string function)
            {
                Component = component;
                Function = function;
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
                return typeof(Component).Name + "/" + Function;
            }

            private void EnsureInitialized()
            {
                if (_initialized) return;
                
                _methodInfo = Component.GetType().GetMethod(Function);
                _initialized = true;
            }

            [Button]
            private void Test() => Debug.Log(this.GetType().ToString());
        }

        [System.Serializable]
        private class FunctionCall<T> : FunctionCall
        {
            public ValueObject<T> Parameter1;

            public FunctionCall (Component component, string function) : base(component, function)
            {

            }

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
}
