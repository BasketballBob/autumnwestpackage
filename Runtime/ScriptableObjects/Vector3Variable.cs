using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class Vector3Variable : ScriptableVariable<Vector3>
    {
        private const string CreateItemName = "Vector3Variable";
    }
}
