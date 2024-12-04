using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class FloatVariable : ScriptableVariable<float>
    {
        private const string CreateItemName = "FloatVariable";

        // public override string ToString(string format, IFormatProvider formatProvider)
        // {
        //     return Runtime
        // }
    }
}
