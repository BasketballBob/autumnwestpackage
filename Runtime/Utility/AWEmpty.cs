using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public class AWEmpty : MonoBehaviour
    {
        // THIS CLASS IS LITERALLY JUST AN EMPTY MONOBEHAVIOUR
        // Used primarily for a component to call MonoBehaviour functions on
        public ParticleSystem.MinMaxCurve _curve = new ParticleSystem.MinMaxCurve();
        public AnimationCurve beee = new AnimationCurve();
    }
}
