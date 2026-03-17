using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(AWTags))]
    public class CompoundAWTags : MonoBehaviour, IAWTagsReference
    {
        [SerializeField]
        private AWTags _tagsReference;
        [SerializeField]
        private List<GameObject> _attachedObjects = new List<GameObject>();

        public AWTags Tags => _tagsReference;

        private void Reset()
        {
            _tagsReference = GetComponent<AWTags>();
        }

        private void Awake()
        {
            _attachedObjects.ForEach(x =>
            {
                x.AddComponent<AWTagsPointer>().Reference = this;
            });
        }
    }
}
