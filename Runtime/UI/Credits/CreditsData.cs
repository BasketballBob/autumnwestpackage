using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    [CreateAssetMenu(fileName = CreateItemName, menuName = CreateFolderName + CreateItemName)]
    public class CreditsData : AWScriptableObject
    {
        private const string CreateItemName = "CreditsData";
        private const string ResourcesFolder = "CreditsData/";

        [HideReferenceObjectPicker]
        public List<CreditsItem> CreditsItems = new List<CreditsItem>();

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format, formatProvider);
        }

        public abstract class CreditsItem
        {
            public abstract List<CreditsEntry> GetCreditEntries();
        }

        public class CreditsSection : CreditsItem
        {
            public string Title;
            [TextArea(1, 20)]
            public string Content;

            public override List<CreditsEntry> GetCreditEntries()
            {
                List<CreditsEntry> entries = new List<CreditsEntry>();
                entries.Add(new HeaderObject(Title));
                Content.Split('\n').ForEach(x =>
                {
                    entries.Add(new BodyObject(x));
                });

                return entries;
            }
        }

        public class CreditsImage : CreditsItem
        {
            [PreviewField]
            public Sprite Image;
            public float Height;

            public override List<CreditsEntry> GetCreditEntries()
            {
                return new List<CreditsEntry>() { new ImageObject(Image, Height) };
            }
        }

        public static CreditsData LoadCreditsData(string name)
        {
            return Resources.Load<CreditsData>(ResourcesFolder + name);
        }
    }
}
