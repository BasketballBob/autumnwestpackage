using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class UnparentOnStart : MonoBehaviour
    {
        private void Start()
        {
            transform.SetParent(null);
        }
    }
}
