using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMODUnity;
using UnityEngine;

namespace AWP
{
    public class OneShotPlayer : MonoBehaviour
    {
        [SerializeField]
        private ItemSelector<EventReference> _eventReference;

        public void Play()
        {
            AWGameManager.AudioManager.PlayOneShot(_eventReference.GetItem());
        }

        public void PlayAttached()
        {
            AWGameManager.AudioManager.PlayOneShotAttached(_eventReference.GetItem(), gameObject);
        }

        public void PlayAtWorldPos(Vector3 worldPos)
        {
            AWGameManager.AudioManager.PlayOneShot(_eventReference.GetItem(), worldPos);
        }
        public void PlayAtWorldPos() => PlayAtWorldPos(transform.position);
    }
}
