using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class AWScriptableObject : ScriptableObject, IFormattable
    {
        public Action OnValueChanged;

        public abstract string ToString(string format, IFormatProvider formatProvider);

        public string ToString(string format)
        {
            return ToString(format, System.Globalization.CultureInfo.CurrentCulture);
        }

        public override string ToString()
        {
            return ToString(null);
        }
    }
}
