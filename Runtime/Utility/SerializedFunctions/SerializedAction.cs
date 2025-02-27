using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AWP
{
    [InlineProperty]
    public class SerializedAction
    {
        [SerializeField] [HideLabel] [InlineProperty] 
        private GameObject _gameObject;
        [OdinSerialize] [HideLabel] [InlineProperty] [ShowIf("@_gameObject != null")] [ValueDropdown("GetAllFunctions")] //[CustomValueDrawer("TestValueDrawer")]
        private ActionCall _actionCall;

        public void Invoke()
        {
            _actionCall.Invoke();
        }

        public ValueDropdownList<ActionCall> GetAllFunctions()
        {
            if (_gameObject == null) return null;

            ValueDropdownList<ActionCall> returnList = new ValueDropdownList<ActionCall>();

            foreach (Component component in _gameObject.GetComponents<Component>())
            {
                foreach (MethodInfo methodInfo in component.GetType().GetMethods())
                {
                    string dropdownName = methodInfo.DeclaringType.Name + "/" + methodInfo.Name;
                    ActionCall functionCall = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    if (parameters.Length > 0 && methodInfo.IsPublic)
                    {
                        try
                        {
                            Type genericType = typeof(ActionCall<>).MakeGenericType(new Type[] { parameters[0].ParameterType });
                            functionCall = Activator.CreateInstance(genericType, new object[] { component, methodInfo.Name }) as ActionCall;
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"TYPE CAUGHT ERROR {parameters[0].ParameterType} " + e);
                            continue;
                        }
                    }
                    else functionCall = new ActionCall(component, methodInfo.Name);

                    returnList.Add(new ValueDropdownItem<ActionCall>(dropdownName, functionCall));
                }
            }

            return returnList;
        }
    }
}
