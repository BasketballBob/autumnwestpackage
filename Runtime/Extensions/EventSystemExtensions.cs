using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AWP
{
    public static class EventSystemExtensions
    {
        public static List<RaycastResult> GetCurrentRaycastResults(this EventSystem eventSystem, Vector2 mousePos)
        {
            PointerEventData pointer = new PointerEventData(eventSystem);
            pointer.position = mousePos;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(pointer, raycastResults);

            return raycastResults;
        }

        // public static RaycastResult GetCurrentRaycastResult(this EventSystem eventSystem, Vector2 mousePos)
        // {
        //     return GetCurrentRaycastResults(eventSystem, mousePos)
        // }
    }
}
