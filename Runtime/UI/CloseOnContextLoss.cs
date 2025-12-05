using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AWP
{
    /// <summary>
    /// from: https://discussions.unity.com/t/how-to-close-a-ui-panel-when-clicking-outside/578684/10
    /// Thank you The-Grand-Memige <3
    /// </summary>
   public class CloseOnContextLoss : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool inContext;
        private GameObject myGO;

        private void Awake()
        {
            myGO = gameObject;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !inContext)
            {
                myGO.SetActive(inContext);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            inContext = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inContext = false;
        }
    }
}
