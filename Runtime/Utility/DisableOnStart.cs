using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class DisableOnStart : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
