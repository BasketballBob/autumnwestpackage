using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AWP
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownSFX : MonoBehaviour
    {
        [SerializeField]
        private EventReference _valueChangeSFX;

        private TMP_Dropdown _dropdown;

        private void OnEnable()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.onValueChanged.AddListener(PlayValueChangeSFX);
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveListener(PlayValueChangeSFX);
        }

        private void PlayValueChangeSFX(int x) => AWGameManager.AudioManager.PlayOneShot(_valueChangeSFX);
    }
}
