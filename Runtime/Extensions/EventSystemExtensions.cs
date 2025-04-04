using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public static Selectable GetTopRaycastSelectable(this EventSystem eventSystem, Vector2 mousePos)
        {
            List<RaycastResult> results = eventSystem.GetCurrentRaycastResults(mousePos);
            if (results.IsNullOrEmpty()) return null;
            Selectable selectableResult;

            for (int i = 0; i < results.Count; i++)
            {
                if (!results[i].isValid) continue;

                selectableResult = results[i].gameObject.transform.GetRootComponent<Selectable>();
                if (selectableResult != null) return selectableResult;
            }

            return null;
        }
    }
}
