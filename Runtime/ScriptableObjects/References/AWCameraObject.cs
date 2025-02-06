using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class AWCameraObject : ReferenceObject<AWCamera>
    {
        private const string CreateItemName = "AWCameraObject";
    }
}
