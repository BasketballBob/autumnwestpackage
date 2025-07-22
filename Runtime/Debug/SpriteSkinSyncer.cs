using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace AWP
{
    public class SpriteSkinSyncer : MonoBehaviour
    {
        [SerializeField]
        private Transform _rootBone;
        [SerializeField]
        private List<Transform> _transforms = new List<Transform>();

        [Button]
        private void Sync()
        {
            Transform currentBone = _rootBone;
            int transformIndex = 0;

            while (currentBone != null)
            {
                Debug.Log($"WEST {transformIndex} {currentBone.name}");
                _transforms[transformIndex].transform.position = currentBone.transform.position;

                if (currentBone.childCount <= 0) return;
                currentBone = currentBone.GetChild(0);
                transformIndex++;
            }
        }
    }
}
