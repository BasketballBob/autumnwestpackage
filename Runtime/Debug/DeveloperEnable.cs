using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    /// <summary>
    /// Disables the object if not in developer mode
    /// </summary>
    public class DeveloperEnable : MonoBehaviour
    {
        [SerializeField]
        private DeveloperMode _minDevMode = DeveloperMode.Developer;

        private void Start()
        {
            if (!AWGameManager.IsMinimumMode(_minDevMode))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
