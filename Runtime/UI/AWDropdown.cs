using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AWP
{
    public class AWDropdown : AWUI
    {
        [SerializeField]
        protected TMP_Dropdown _dropdown;

        private void Reset()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        private void OnEnable()
        {
            _dropdown.onValueChanged.AddListener(OnDropdownChange);
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveListener(OnDropdownChange);
        }

        protected virtual void OnDropdownChange(int newIndex)
        {

        }
    }

    public abstract class AWDropdown<TValue> : AWDropdown
    {
        protected virtual LabeledList<TValue> ItemList => null;

        protected virtual void Start()
        {
            InitializeOptions(ItemList);
        }

        protected override void OnDropdownChange(int newIndex)
        {
            OnValueChanged(ItemList[newIndex].Value);
        }

        protected abstract void OnValueChanged(TValue newValue);
        /// <summary>
        /// Gets the value that the dropdown should be initialized to
        /// </summary>
        /// <returns></returns>
        protected abstract TValue GetStartValue();

        public void InitializeOptions(LabeledList<TValue> values)
        {
            _dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();
            values.ForEach(x =>
            {
                optionData.Add(new TMP_Dropdown.OptionData(x.Label));
            });
            _dropdown.AddOptions(optionData);

            InitializeStartOption();
        }

        private void InitializeStartOption()
        {
            int index = ItemList.IndexOf(GetStartValue());
            _dropdown.SetValueWithoutNotify(index);
        }
    }
}
