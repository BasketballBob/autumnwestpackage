using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AWP
{
    [RequireComponent(typeof(Button))]
    public class ButtonSFX : HoverSFX
    {
        [SerializeField]
        private EventReference _selectSFX;

        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlaySelectSFX);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlaySelectSFX);
        }

        private void PlaySelectSFX() => AWGameManager.AudioManager.PlayOneShot(_selectSFX);
    }
}
