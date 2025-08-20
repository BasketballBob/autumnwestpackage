using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    public abstract class AWScriptableObject : SerializedScriptableObject, IFormattable
    {
        [NonSerialized]
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
