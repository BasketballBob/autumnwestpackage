using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class FormattableFunc<T> : IFormattable where T : IFormattable
    {
        public Func<T> Function;

        public FormattableFunc(Func<T> function)
        {
            Function = function;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Function().ToString(format, formatProvider);
        }
    }
}
