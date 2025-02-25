using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class AWRandom
    {
        public static float Range01()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

        public static float RangeSigned1()
        {
            return UnityEngine.Random.Range(-1f, 1f);
        }
    }
}
