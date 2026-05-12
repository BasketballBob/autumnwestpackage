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

        public bool SelectSFXDisabled { get; set; }

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            InitializeListeners();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlaySelectSFX);
        }

        /// <summary>
        /// Initializes the listeners for the button sfx
        /// (Added the ability to call this for the characterSelectButton in chicken game that removes all listeners)
        /// </summary>
        public void InitializeListeners()
        {
            _button.onClick.AddListener(PlaySelectSFX);
        }

        protected override void PlayHoverSFX()
        {
            if (!_button.IsInteractable()) return;
            base.PlayHoverSFX();
        }
        private void PlaySelectSFX()
        {
            if (SelectSFXDisabled) return;
            AWGameManager.AudioManager.PlayOneShot(_selectSFX);
        }
    }
}
