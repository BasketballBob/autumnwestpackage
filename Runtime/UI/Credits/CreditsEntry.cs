using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AWP
{
    /// <summary>
    /// Used for the prefabs that the CreditsManager creates
    /// </summary>
    [System.Serializable]
    public abstract class CreditsEntry
    {
        //public CreditsObject Item;
        //public CreditsEntry Entry;
        public float Height;
        public float Position;
        public ComponentPool<CreditsObject> Pool;
    }

    [System.Serializable]
    public class HeaderObject : CreditsEntry
    {
        public string Text;

        public HeaderObject(string text)
        {
            Text = text;
        }
    }

    [System.Serializable]
    public class BodyObject : CreditsEntry
    {
        public string Text;

        public BodyObject(string text)
        {
            Text = text;
        }
    }
}
