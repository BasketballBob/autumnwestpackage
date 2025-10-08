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

        public void OnPointerEnter(PointerEventData eventData)
        {
            AWGameManager.AudioManager.PlayOneShot(_hoverSFX);
        }
    }
}
