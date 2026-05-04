using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class ToggleSFX : MonoBehaviour
    {
        [SerializeField]
        private Toggle _toggle;
        [SerializeField]
        private EventReference _onTrue;
        [SerializeField]
        private EventReference _onFalse;

        private void Reset()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(PlaySFX);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(PlaySFX);
        }

        private void PlaySFX(bool value)
        {
            AWGameManager.AudioManager.PlayOneShotAttached(value ? _onTrue : _onFalse, gameObject);
        }
    }
}
