using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AWP
{
    /// <summary>
    /// Used specifically to deal with dropdown button reopening menu
    /// </summary>
    public class OnContextLossDropdown : OnContextLoss
    {
        [SerializeField]
        private TMP_Dropdown _dropdown;

        private SingleCoroutine _lossRoutine;

        private void Start()
        {
            _lossRoutine = new SingleCoroutine(_dropdown);
        }

        protected override void OnLoss()
        {
            base.OnLoss();
            _lossRoutine.StartRoutine(LossRoutine());
        }

        private IEnumerator LossRoutine()
        {
            _dropdown.Hide();
            _dropdown.interactable = false;
            while (!_inputAction.action.WasReleasedThisFrame()) yield return null;
            _dropdown.interactable = true;
        }
    }
}
