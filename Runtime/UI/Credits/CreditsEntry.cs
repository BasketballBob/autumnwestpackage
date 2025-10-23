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
        // //public CreditsObject Item;
        // //public CreditsEntry Entry;
        // public float Height;
        // public float Position;

        public float Size;
        public CreditsObject Instance;
        public ComponentPool<CreditsObject> Pool;

        public void EnsureInstance()
        {
            if (Instance != null) return;
            Instance = Pool.PullObject();
        }

        public void DisposeInstance()
        {
            if (Instance == null) return;
            Pool.DisposeObject(Instance);
            Instance = null;
        }
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

    [System.Serializable]
    public class ImageObject : CreditsEntry
    {
        public Sprite Image;
        public float ImageHeight;

        public ImageObject(Sprite image, float imageHeight)
        {
            Image = image;
            ImageHeight = imageHeight;
        }
    }
}
