using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class CreditsData : AWScriptableObject
    {
        private const string CreateItemName = "CreditsData";

        public List<CreditsItem> CreditsItems = new List<CreditsItem>();

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format, formatProvider);
        }

        public abstract class CreditsItem
        {

        }

        public class CreditsSection : CreditsItem
        {
            public string Title;
            [TextArea(1, 20)]
            public string Content;
        }
    }
}
