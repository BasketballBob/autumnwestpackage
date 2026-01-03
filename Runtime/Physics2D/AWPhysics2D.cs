using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace AWP
{
    public static class AWPhysics2D
    {
        public static void Explosion(Vector2 point, float radius, float magnitude) => Explosion(point, radius, magnitude, ~0);
        public static void Explosion(Vector2 point, float radius, float magnitude, LayerMask layerMask, Action<Rigidbody2D, Vector2> hitAction = null)
        {
            Collider2D[] colArray = Physics2D.OverlapCircleAll(point, radius, layerMask);
            Debug.DrawLine(point, point - Vector2.right * radius, Color.red, 1);

            List<Rigidbody2D> bodies = new List<Rigidbody2D>();
            colArray.ForEach(x =>
            {
                if (x.attachedRigidbody == null) return;
                bodies.Add(x.attachedRigidbody);
            });

            foreach (Rigidbody2D body in bodies)
            {
                Vector2 difference = (Vector2)body.worldCenterOfMass - point;
                if (difference.magnitude == 0) continue;
                Vector2 appliedForce = difference.normalized * magnitude;

                hitAction?.Invoke(body, appliedForce);
                body.AddForceAtPosition(appliedForce, body.ClosestPoint(point), ForceMode2D.Impulse);
                Debug.DrawRay(body.ClosestPoint(point), appliedForce, Color.magenta, 1);
            }
        }

        /// <summary>
        /// Use AddTorque() to face a specific angle. For 2D physiscs. Should be called in every FixedUpdate() frame.
        /// Ripped from: https://discussions.unity.com/t/rotate-a-2d-rigidbody-to-a-desired-angle-using-addtorque/157633/4
        /// </summary>
        /// <param name="currentVec"> vector representing the direction we are currently pointing at. (transform.right) </param>
        /// <param name="targetVec"> vector representing the direction we want to point at. </param>
        /// <param name="rb"> Rigidbody to affect. </param>
        /// <param name="maxTorque"> Max torque to apply. </param>
        /// <param name="torqueDampFactor"> Damping factor to avoid undershooting. </param>
        /// <param name="offsetForgive"> Stop applying force when the angles are within this threshold (default 0). </param>
        public static void TorqueTo(this Rigidbody2D rb, float angleDifference, float maxTorque, float torqueDampFactor, float offsetForgive = 0)
        {
            // if (Mathf.Abs(angleDifference) < offsetForgive) return;

            // float torqueToApply = maxTorque * angleDifference / 180f;
            // torqueToApply -= rb.angularVelocity * torqueDampFactor;
            // rb.AddTorque(torqueToApply, ForceMode2D.Force);

            throw new NotImplementedException();
        }

        #region Collider2D
        //public static Vector2 ClosestPoint(this Collider2D col, Vector2 nearPosition, Vector2 colPosition, )
        #endregion

        #region Rigidbody
        public static Vector2 GetPointVelocity(this Rigidbody2D rb, Vector2 pos, Vector2 velocity, float angularVelocity)
        {
            if (rb.bodyType != RigidbodyType2D.Dynamic) return default;

            Vector2 oldVelocity = rb.velocity;
            float oldAngularVelocity = rb.angularVelocity;

            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;

            Vector2 pointVelocity = rb.GetPointVelocity(pos);

            rb.velocity = oldVelocity;
            rb.angularVelocity = oldAngularVelocity;

            return pointVelocity;
        }

        public static void IgnoreRigidbodyCollision(Rigidbody2D rb1, Rigidbody2D rb2, bool ignore = true)
        {
            Collider2D[] col2 = rb2.GetAttachedColliders();

            for (int i = 0; i < col2.Length; i++)
            {
                IgnoreRigidbodyColliderCollision(rb1, col2[i], ignore);
            }
        }

        public static void IgnoreRigidbodyColliderCollision(Rigidbody2D rb, Collider2D col, bool ignore = true)
        {
            Collider2D[] attachedCols = rb.GetAttachedColliders();
            for (int i = 0; i < attachedCols.Length; i++)
            {
                Physics2D.IgnoreCollision(attachedCols[i], col, ignore);
                //Debug.Log($"IGNORE RIGIDBODY COLLIDER COLLISION col:{col.name}-{col}   rb:{rb.name}-{attachedCols[i]}");
            }
        }
        #endregion

        #region Collision detections
        /// <summary>
        /// 
        /// Referenced: https://www.jeffreythompson.org/collision-detection/line-line.php
        /// </summary>
        /// <returns></returns>
        public static Vector2? LineLineIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
        {
            float x1 = line1Start.x;
            float y1 = line1Start.y;
            float x2 = line1End.x;
            float y2 = line1End.y;
            float x3 = line2Start.x;
            float y3 = line2Start.y;
            float x4 = line2End.x;
            float y4 = line2End.y;

            // Calculate distance to the intersection point
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // If there is a collision, uA and uB should both be in the range of 0-1
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                float intersectionX = x1 + (uA * (x2 - x1));
                float intersectionY = y1 + (uA * (y2 - y1));

                return new Vector2(intersectionX, intersectionY);
            }

            return null;
        }

        public static bool LineLineCollision(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
        {
            return LineLineIntersection(line1Start, line1End, line2Start, line2End) != null;
        }

        public static Vector2 LineClosestPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            float closestDist = LineClosestDistance(lineStart, lineEnd, point);
            float x = lineStart.x + closestDist * (lineEnd.x - lineStart.x);
            float y = lineStart.y + closestDist * (lineEnd.y - lineStart.y);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns the closest distance between a line and a point
        /// Referenced: https://paulbourke.net/geometry/pointlineplane/
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float LineClosestDistance(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            return ((point.x - lineStart.x) * (lineEnd.x - lineStart.x) +
                (point.y - lineStart.y) * (lineEnd.y - lineStart.y)) /
                Mathf.Pow((lineEnd - lineStart).magnitude, 2);
        }
        #endregion

        #region Calculations
        /// <summary>
        /// 
        /// Borrowed from here: https://www.youtube.com/watch?v=RpeRnlLgmv8
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="pos"></param>
        /// <param name="velocity"></param>
        /// <param name="steps"></param>
        /// <param name="mazSize">The maximum</param> 
        /// <returns></returns>
        public static Vector2[] PlotTrajectory(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int maxSteps, Vector2 maxExtents)
        {
            float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
            Vector2 gravityAccel = Physics2D.gravity * rb.gravityScale * timestep;
            float drag = 1f - timestep * rb.drag;

            return PlotFunc(new PlotFunctionData(Vector2.zero, velocity), (data) =>
            {
                data.MoveStep += gravityAccel;
                data.MoveStep *= drag;
                data.LocalPos += data.MoveStep * timestep;
            }, pos, maxSteps, maxExtents);
        }

        public static Vector2[] PlotFunc(Action<PlotFunctionData> func, Vector2 pos, int maxSteps, Vector2 maxExtents)
        {
            return PlotFunc(new PlotFunctionData(), func, pos, maxSteps, maxExtents);
        }
        public static Vector2[] PlotFunc(PlotFunctionData data, Action<PlotFunctionData> func, Vector2 pos, int maxSteps, Vector2 maxExtents)
        {
            List<Vector2> results = new List<Vector2>();

            results.Add(pos);

            for (int i = 0; i < maxSteps - 1; i++)
            {
                func(data);
                if (CheckToClampFunc()) break;
                results.Add(data.LocalPos + pos);
            }

            return results.ToArray();

            bool CheckToClampFunc()
            {
                ;
                float angle = Mathf.Atan2(data.LocalPos.y, data.LocalPos.x);
                float magnitude = maxExtents.x + (maxExtents.y - maxExtents.x) * Mathf.Cos(angle);
                Vector3 maxExtentPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
                Vector3 absExtentPoint = new Vector2(Mathf.Abs(maxExtentPoint.x), Mathf.Abs(maxExtentPoint.y));

                //Debug.DrawLine(Vector3.zero, Vector3.zero + maxExtentPoint, Color.black);
                //Debug.DrawLine(Vector3.zero, Vector3.zero + (Vector3)data.LocalPos, Color.magenta);

                if (Mathf.Abs(data.LocalPos.x) < absExtentPoint.x &&
                    Mathf.Abs(data.LocalPos.y) < absExtentPoint.y)
                    return false;

                float xOutsideDist = Mathf.Abs(data.LocalPos.x) - absExtentPoint.x;
                float yOutsideDist = Mathf.Abs(data.LocalPos.y) - absExtentPoint.y;

                if (absExtentPoint.x >= absExtentPoint.y)
                {
                    float clampedY = absExtentPoint.x * (data.LocalPos.normalized.y / Mathf.Abs(data.LocalPos.normalized.x));
                    clampedY = Mathf.Clamp(clampedY, -absExtentPoint.x, absExtentPoint.x);

                    results.Add(new Vector2(absExtentPoint.x * AWUnity.SignWithZero(data.LocalPos.x), clampedY) + results.FirstItem());
                }
                else
                {
                    float clampedX = absExtentPoint.y * (data.LocalPos.normalized.x / Mathf.Abs(data.LocalPos.normalized.y));
                    clampedX = Mathf.Clamp(clampedX, -absExtentPoint.x, absExtentPoint.x);

                    results.Add(new Vector2(clampedX, absExtentPoint.y * AWUnity.SignWithZero(data.LocalPos.y)) + results.FirstItem());
                }

                return true;
            }
        }
        #endregion
    }
}
