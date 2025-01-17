using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class UnparentOnStart : MonoBehaviour
    {
        [SerializeField]
        private UnparentType _unparentType;

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
                    transform.SetParent(null);
                    break;
                case UnparentType.Children:
                    transform.UnparentChildren();
                    break;
            }
        }
    }
}
