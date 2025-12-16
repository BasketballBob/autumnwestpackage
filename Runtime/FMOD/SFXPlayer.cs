using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace AWP
{
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField]
        private LabeledList<EventReference> _events;
        [SerializeReference]
        private ITransformReference _attachedTransformOverride;

        public ITransformReference AttachedTransformOverride { get => _attachedTransformOverride; set => _attachedTransformOverride = value; }
        private Transform AttachedTransform => _attachedTransformOverride != null ? _attachedTransformOverride.Transform : transform;

        public void PlayOneShot(string name)
        {
            AWGameManager.AudioManager.PlayOneShot(_events.GetItem(name));
        }

        public void PlayOneShotAttached(string name)
        {
            AWGameManager.AudioManager.PlayOneShotAttached(_events.GetItem(name), AttachedTransform.gameObject);
        }
    }
}
