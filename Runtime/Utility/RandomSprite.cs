using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AWP
{
    public class RandomSprite : MonoBehaviour
    {
        private static Dictionary<string, List<Sprite>> DuplicateList = new Dictionary<string, List<Sprite>>();

        [SerializeField]
        private SpriteRenderer _sr;
        [SerializeField]
        private WeightedItemPool<Sprite> _sprites = new WeightedItemPool<Sprite>();
        [SerializeField]
        private bool _deprioritizeRepeats;
        [SerializeField]
        [ShowIf("@_deprioritizeRepeats")]
        private string _guid;

#if UNITY_EDITOR
        [Button]
        [ShowIf("@_deprioritizeRepeats")]
        private void GenerateNewGUID()
        {
            _guid = Guid.NewGuid().ToString();

            EditorUtility.SetDirty(this);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
#endif

        private void Start()
        {
            if (_deprioritizeRepeats)
            {
                if (!DuplicateList.ContainsKey(_guid))
                {
                    DuplicateList.Add(_guid, new List<Sprite>());
                }
            }

            _sr.sprite = PullSprite();
        }

        private Sprite PullSprite()
        {
            if (!_deprioritizeRepeats) return _sprites.PullItem();

            if (DuplicateList[_guid].Count >= _sprites.Count)
            {
                DuplicateList[_guid].Clear();
            }

            Sprite pulledSprite = _sprites.PullItemConditionally(x =>
            {
                if (DuplicateList[_guid].Contains(x)) return false;
                return true;
            });

            DuplicateList[_guid].Enqueue(pulledSprite);

            return pulledSprite;
        }
    }
}
