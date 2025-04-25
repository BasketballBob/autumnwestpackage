using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AWP
{
    public static class AWOdin
    {
        /// <summary>
        /// Get a valuedropdown list with all of the sorting layers 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable SortingLayers()
        {
            return SortingLayer.layers.Select(x => new ValueDropdownItem(x.name, x.id));
        }
    }
}
