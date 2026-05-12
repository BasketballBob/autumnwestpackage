using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;

namespace AWP
{
    public class HoverSFX : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField]
        private EventReference _hoverSFX;

        public bool HoverSFXDisabled { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayHoverSFX();
        }

        protected virtual void PlayHoverSFX()
        {
            if (HoverSFXDisabled) return;
            AWGameManager.AudioManager.PlayOneShot(_hoverSFX);
        }
    }
}
