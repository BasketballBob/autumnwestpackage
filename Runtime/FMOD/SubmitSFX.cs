using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

namespace AWP
{
    public class SubmitSFX : MonoBehaviour, ISubmitHandler
    {
        [SerializeField]
        private EventReference _submitSFX;

        public void OnSubmit(BaseEventData eventData)
        {
            Debug.Log($"SUBMIT {name}");
            AWGameManager.AudioManager.PlayOneShot(_submitSFX);
        }
    }
}
