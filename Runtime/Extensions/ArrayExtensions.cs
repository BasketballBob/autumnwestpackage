using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Shifts the index of all elements by offset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Array ShiftIndex<T>(this T[] array, int offset)
        {
            T[] returnArray = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                if (i + offset >= 0 && i + offset < array.Length)
                {
                    returnArray[i + offset] = array[i];
                }
            }

            return returnArray;
        }
    }
}
