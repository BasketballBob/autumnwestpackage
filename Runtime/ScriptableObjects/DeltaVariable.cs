using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class DeltaVariable : FloatVariable, IDelta
    {
        private const string CreateItemName = "DeltaVariable";

        [Header("Delta variables")]
        public float MinValue = 0;
        public float MaxValue = 1;

        public float Delta
        {
            get => Mathf.Clamp01((RuntimeValue - MinValue) / (MaxValue - MinValue));
            set
            {
                RuntimeValue = MinValue.Lerp(MaxValue, value);
            }
        }

        // public override float RuntimeValue 
        // { 
        //     get => base.RuntimeValue; 
        //     set => base.RuntimeValue = Mathf.Clamp(value, MinValue, MaxValue); 
        // }
        
    }
}
