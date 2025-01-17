using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class WeightedPrefabPool : MonoBehaviour
    {
        [SerializeField] [AssetsOnly]
        private WeightedItemPool<GameObject> _prefabPool;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("INITIALIZE");
            GameObject prefab = Instantiate(_prefabPool.PullItem(), transform);
            prefab.transform.SetParent(transform.parent);
            Destroy(gameObject);
        }
    }
}
