using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class SpriteRendererExtensions
    {
        /// <summary>
        /// Copies the sorting settings from sr2 to sr1
        /// </summary>
        /// <param name="sr1"></param>
        /// <param name="sr2"></param>
        public static void CopySortingSettings(this SpriteRenderer sr1, SpriteRenderer sr2)
        {
            sr1.sortingLayerID = sr2.sortingLayerID;
            sr1.sortingOrder = sr2.sortingOrder;
        }
    }
}
