using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AWP
{
    public static class HingeJoint2DExtensions
    {
        public static void RotateTowardsAngle(this HingeJoint2D hinge, float targetEulerZ, float speed) //, float maxTorque = 999)
        {
            float shortestRotation = AWUnity.GetShortestRotation(hinge.transform.eulerAngles.z + hinge.attachedRigidbody.angularVelocity * Time.fixedDeltaTime, targetEulerZ);


            hinge.useMotor = true;

            JointMotor2D motor = hinge.motor;
            //motor.maxMotorTorque = maxTorque;

            float maxRot = shortestRotation / Time.fixedDeltaTime;

            motor.motorSpeed = shortestRotation * speed;
            if (Mathf.Abs(motor.motorSpeed) > Mathf.Abs(maxRot)) motor.motorSpeed = maxRot;

            motor.motorSpeed *= -1;

            // Apply drag
            //motor.motorSpeed = motor.motorSpeed * (1 - Time.fixedDeltaTime * motorDrag);
            //moveSpeed = moveSpeed * (1 - Time.fixedDeltaTime * TopSpeedDrag);


            hinge.motor = motor;
        }

        public static Vector2 GetConnectedAnchorWorldPosition(this HingeJoint2D joint)
        {
            return joint.connectedBody.transform.TransformPoint(joint.connectedAnchor);
        }

        /// <summary>
        /// Moves the provided joint to the world position where anchor and connectedAnchor overlap
        /// </summary>
        /// <param name="joint"></param>
        public static void ResetPosition(this HingeJoint2D joint)
        {
            Vector2 targetPos = joint.GetConnectedAnchorWorldPosition();
            Vector2 anchorOffset = joint.transform.position - joint.transform.TransformPoint(joint.anchor);
            
            joint.enabled = false;
            joint.attachedRigidbody.transform.position = targetPos + anchorOffset;
            joint.enabled = true;
        }
    }
}
