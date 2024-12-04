using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class TrackedVariable<T> : TrackedVariable where T : IFormattable
    {
        public override IFormattable Type => typeof(T) as IFormattable;

        public TrackedVariable(string name, IFormattable variable) : base(name, variable)
        {

        }
    }

    public abstract class TrackedVariable
    {
        public string Name;
        public IFormattable Variable;
        public abstract IFormattable Type { get; }

        public TrackedVariable(string name, IFormattable variable)
        {
            Name = name;
            Variable = variable;
        }
    }
}
