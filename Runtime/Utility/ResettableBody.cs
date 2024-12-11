using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class ResettableBody : MonoBehaviour
    {
        [SerializeField]
        protected bool _zeroVelocity = true;
        [SerializeField]
        protected bool _zeroAngularVelocity = true;
    }
}
