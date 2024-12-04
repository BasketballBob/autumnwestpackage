using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public abstract class CurrencyVariable<T> : ScriptableVariable<T> where T : IComparable
    {
        public bool CanAfford(T cost)
        {   
            return cost.CompareTo(RuntimeValue) > 0;
            //float
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Whether the purchase is successful or not</returns>
        public bool AttemptPurchase(T cost)
        {
            if (!CanAfford(cost)) return false;

            Purchase(cost);
            return true;
        }

        public void Purchase(T cost)
        {
            dynamic runtimeVal = (dynamic)RuntimeValue;
            dynamic costVal = (dynamic)cost;

            RuntimeValue = runtimeVal - costVal;
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return base.ToString(format, formatProvider);
        }
    } 
}
