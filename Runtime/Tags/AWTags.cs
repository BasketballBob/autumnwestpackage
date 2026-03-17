using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.ObjectModel;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{

    public class AWTags : MonoBehaviour, IAWTagsReference
    {
        [SerializeField]
        private TagCollection _collection;
        [SerializeField]
        [CustomValueDrawer("TagMask")]
        private int _appliedTags;

        public AWTags Tags => this;
        public TagCollection Collection { get { return _collection; } }
        public int AppliedTags { get { return _appliedTags; } }

        protected virtual void OnEnable()
        {
            _collection.AddInstance(this);
            //Debug.Log($"ADD INSTANCE {this}");
        }

        protected virtual void OnDisable()
        {
            _collection.RemoveInstance(this);
            //Debug.Log($"REMOVE INSTANCE {this}");
        }

        public bool FitsMask(int mask) => Collection.ItemFitsMask(gameObject, mask);
        public bool HasTag(string tag) => Collection.ItemHasTag(gameObject, tag);

        private IEnumerable TagOptions()
        {
            if (_collection == null) return null;
            return _collection.GetTags();
        }

        public static bool ItemHasTag(GameObject gameObject, string tagName)
        {
            if (!gameObject.TryGetComponent<IAWTagsReference>(out IAWTagsReference tagsRef)) return false;
            return tagsRef.Tags.HasTag(tagName);
        }

        public static bool ItemFitsMask(GameObject gameObject, int mask)
        {
            if (!gameObject.TryGetComponent<IAWTagsReference>(out IAWTagsReference tagsRef)) return false;
            return tagsRef.Tags.FitsMask(mask);
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
