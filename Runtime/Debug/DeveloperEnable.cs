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
        [SerializeField]
        private Mode _mode;
        [SerializeField]
        private bool _disableInstead;

        private enum Mode { IsMinimum, IsMaximum };

        private void Start()
        {
            switch (_mode)
            {
                case Mode.IsMinimum:
                    if (!AWGameManager.IsMinimumMode(_minDevMode))
                    {
                        gameObject.SetActive(!_disableInstead);
                    }
                    break;
                case Mode.IsMaximum:
                    if (!AWGameManager.IsMaximumMode(_minDevMode))
                    {
                        gameObject.SetActive(!_disableInstead);
                    }
                    break;
            }
        }
    }
}
