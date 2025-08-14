using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System;

namespace AWP
{
    public static class FMODExtensions
    {
        public static FMOD.VECTOR ConvertToFMOD(this Vector3 vector)
        {
            FMOD.VECTOR fmodVector;
            fmodVector.x = vector.x;
            fmodVector.y = vector.y;
            fmodVector.z = vector.z;

            return fmodVector;
        }

        public static Vector3 ConvertToUnity(this FMOD.VECTOR fmodVector)
        {
            return new Vector3(fmodVector.x, fmodVector.y, fmodVector.z);
        }
    }
}
