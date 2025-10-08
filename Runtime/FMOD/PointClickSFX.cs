using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AWP
{
    public class PointClickSFX : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private EventReference _clickSFX;

        public void OnPointerClick(PointerEventData eventData)
        {
            AWGameManager.AudioManager.PlayOneShot(_clickSFX);
        }
    }
}
