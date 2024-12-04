using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class FloatCurrencyVariable : CurrencyVariable<float>
    {
        private const string CreateItemName = "FloatCurrencyVariable";
    }
}
