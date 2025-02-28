using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AWP
{
    public class WorldScaleObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _worldScale = Vector3.one;

        [Button()]
        public void SetWorldScale()
        {
            transform.SetLossyScale(_worldScale);
        }
    }
}
