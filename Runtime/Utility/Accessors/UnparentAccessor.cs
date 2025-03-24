using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class UnparentAccessor : MonoBehaviour
    {
        public void UnparentSelf() => transform.SetParent(null);
        
    }
}
