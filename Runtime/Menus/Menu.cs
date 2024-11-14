using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Enable()
        {
            _canvasGroup.interactable = true;
        }

        public virtual void Disable()
        {
            _canvasGroup.interactable = false;
        }
    }
}
