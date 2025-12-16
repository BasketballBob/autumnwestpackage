using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace AWP
{
    /// <summary>
    /// Used to detect the pointer up / down state of UI
    /// </summary>
    public class PointerUpDown : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public UnityEvent<PointerEventData> OnDown;
        public UnityEvent<PointerEventData> OnUp;

        public bool PointerDown { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown = true;
            OnDown.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerDown = false;
            OnUp.Invoke(eventData);
        }
    }
}
