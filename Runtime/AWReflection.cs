using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AWP
{
    public static class AWReflection
    {
        public static List<Type> GetGenericArguments(object obj)
        {
            //usedInterface.GetInterface()

            foreach (Type type in obj.GetType().GetInterfaces())
            {
                Debug.Log($"EEE {type}");
                if (type is ISaveable)
                {
                    Debug.Log($"TYPE {type.IsGenericType}");
                    Debug.Log($"TYPE {type.GetGenericArguments().Length}");
                }
            }

            //List<Type> genericArguments;

            return null;
        }

        public static List<Type> GetInterfaceGenerics(Type interfaceType)
        {
            List<Type> returnList = interfaceType.GetType().GetGenericArguments().ToList();

            Debug.Log($"EAST {interfaceType} {returnList.Count}");

            return returnList;
        }

        public static Type GetGenericInterface(this Type type, string name)
        {
            foreach (Type test in type.GetInterfaces())
            {
                //if (test.Name != name) continue;
                //if (!test.IsGenericType) continue;

                Debug.Log($"HAS INTERFACE {test} {test.Name}");
            }

            return null;
        }
    }
}
