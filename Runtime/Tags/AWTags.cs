using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    
    public class AWTags : MonoBehaviour
    {
        [SerializeField]
        private TagCollection _collection;
        [SerializeField] [CustomValueDrawer("TagMask")]
        private int _appliedTags;

        public TagCollection Collection { get { return _collection; } }
        public int AppliedTags { get { return _appliedTags; } }

        protected virtual void OnEnable()
        {
            _collection.AddInstance(this);
        }

        protected virtual void OnDisable()
        {   
            _collection.RemoveInstance(this);
        }

        public bool FitsMask(int mask) => Collection.ItemFitsMask(gameObject, mask);
        public bool HasTag(string tag) => Collection.ItemHasTag(gameObject, tag);

        private IEnumerable TagOptions()
        {
            if (_collection == null) return null;
            return _collection.GetTags();
        }

        #if UNITY_EDITOR
            public int TagMask(int mask, GUIContent label)
            {
                if (_collection == null) return mask;
                mask = EditorGUILayout.MaskField(label, mask, _collection.GetTags());
                return mask;
            }
        #endif
    }
}
