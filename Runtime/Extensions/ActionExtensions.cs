using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Returns the provided action with the attached oneshot
        /// ONLY MODIFIES THE LOCAL COPY
        /// </summary>
        /// <param name="action"></param>
        /// <param name="oneShot"></param>
        /// <returns></returns>
        public static Action WithOneShot(this Action action, Action oneShot)
        {
            Action newAction = null;
            newAction = () => 
            {
                oneShot?.Invoke();
                action -= newAction;
            };
            action += newAction;

            return action;
        }
    }
}
