using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class SaveSettings : AWScriptableObject
    {
        public const string CreateItemName = "SaveSettings";

        [FolderPath]
        public List<string> ResourceFolders = new List<string>();

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return "SAVESETTINGS.TOSTRING()";
            //throw new NotImplementedException();
        }
    }
}
