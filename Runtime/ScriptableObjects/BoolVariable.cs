using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class BoolVariable : ScriptableVariable<bool>, IBool
    {
        private const string CreateItemName = "BoolVariable";

        public bool EvaluateBool()
        {
            return RuntimeValue;
        }
    }
}
