using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class ComponentListVariable : ListVariable<Component>
    {
        private const string CreateItemName = "ComponentListVariable";
    }
}
