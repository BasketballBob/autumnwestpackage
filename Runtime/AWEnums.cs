using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public enum DeveloperMode { Build, TestBuild, Developer }
    public enum BuildType { Default, Demo }
    public enum DimensionType { XY, XYZ, XZ }
    public enum SpaceType { WorldSpace, LocalSpace }
    public enum PhysicsTarget2D { Rigidbody2D, Collider }

    public static class AWEnums
    {
        /// <summary>
        /// Zeros the vector at any unused axis
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static Vector3 ZeroVector3(this DimensionType dimension, Vector3 vector)
        {
            switch (dimension)
            {
                case DimensionType.XY:
                    return new Vector3(vector.x, vector.y, 0);
                case DimensionType.XYZ: 
                    return vector;
                case DimensionType.XZ:
                    return new Vector3(vector.x, 0, vector.z);
            }

            throw new NotImplementedException();
        }
    }
}
