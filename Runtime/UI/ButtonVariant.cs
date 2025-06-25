using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AWP
{
    public abstract class ButtonVariant : MonoBehaviour
    {
        protected Button _button;

        protected virtual void OnEnable()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnPress);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnPress);
        }

        protected abstract void OnPress();
    }
}
