using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

namespace AWP
{
    public class VariableTrackerDisplay : MonoBehaviour
    {
        private const string Format = "0.00";

        [SerializeField]
        private TMP_Text _tmp;

        public void SetValues(TrackedVariable trackedVariable)
        {
            _tmp.text = $"{trackedVariable.Name} - {trackedVariable.Variable.ToString(Format, CultureInfo.CurrentUICulture)}";
        }
    }
}
