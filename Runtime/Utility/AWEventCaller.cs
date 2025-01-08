using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWEventCaller : MonoBehaviour
    {
        public AWEvent AWEvent;

        public void Invoke()
        {
            AWEvent.Invoke();
        }
    }
}
