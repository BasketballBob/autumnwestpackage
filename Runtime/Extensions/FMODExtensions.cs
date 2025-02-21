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

        public static void SetInstancePosition(this EventInstance instance, Vector3 position)
        {
            Modify3DAttributes(instance, x => x.position = position.ToFMODVector());
        }

        public static void Modify3DAttributes(this EventInstance instance, Action<FMOD.ATTRIBUTES_3D> modifyFunc)
        {
            FMOD.ATTRIBUTES_3D prevAttributes;
            instance.get3DAttributes(out prevAttributes);
            modifyFunc(prevAttributes);
            instance.set3DAttributes(prevAttributes);
        }
    }
}
