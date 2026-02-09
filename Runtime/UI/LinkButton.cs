using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AWP
{
    public class LinkButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private string _url;

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenLink);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenLink);
        }

        private void OpenLink()
        {
            Application.OpenURL(_url);
        }
    }
}
