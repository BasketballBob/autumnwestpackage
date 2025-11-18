using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace AWP
{
    /// <summary>
    /// Used to detect the enter / exit state of a piece of UI
    /// </summary>
    public class PointerEnterExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IgnoreParent;

        public UnityEvent<PointerEventData> OnEnter;
        public UnityEvent<PointerEventData> OnExit;

        public bool PointerInside { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"EVENT DATA {eventData.pointerCurrentRaycast.gameObject}");
            if (eventData.pointerCurrentRaycast.gameObject != this && IgnoreParent) return;

            PointerInside = true;
            OnEnter.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != this && IgnoreParent) return;

            PointerInside = false;
            OnExit.Invoke(eventData);
        }
    }
}
