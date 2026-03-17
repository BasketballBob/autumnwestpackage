using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWTagsPointer : MonoBehaviour, IAWTagsReference
    {
        [SerializeReference] [System.NonSerialized]
        public IAWTagsReference Reference;

        public AWTags Tags => Reference.Tags;
    }
}
