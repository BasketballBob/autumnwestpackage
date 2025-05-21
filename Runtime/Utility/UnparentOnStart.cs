using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class UnparentOnStart : MonoBehaviour
    {
        [SerializeField]
        private UnparentType _unparentType;
        [SerializeField]
        private Transform _customParent;

        private enum UnparentType { Self, Children };

        private void Start()
        {
            Unparent();
        }

        private void Unparent()
        {
            switch (_unparentType)
            {
                case UnparentType.Self:
                    transform.SetParent(_customParent);
                    break;
                case UnparentType.Children:
                    transform.ReparentChildren(_customParent);
                    break;
            }
        }
    }
}
