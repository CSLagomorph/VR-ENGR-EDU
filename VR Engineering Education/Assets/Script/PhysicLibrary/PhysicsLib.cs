using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsLibrary
{
    public static class PhysicsLib
    {
        public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float acceleration = 1, ForceMode mode = ForceMode.Force)
        {
            // No movement
            if (acceleration == 0 || velocity.magnitude == 0)
            {
                return;
            }

            // acceleration dictates the time in second to reach the velocity
            // If mass = 1 then force can be maximum of 1 / Time.fixedDeltaTime
            acceleration = Mathf.Clamp(acceleration, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

            velocity += velocity.normalized * 0.2f * rigidbody.drag;

            // Add force to the object
            if (rigidbody.velocity.magnitude == 0)
            {
                rigidbody.AddForce(velocity * acceleration, mode);
            }

            else
            {
                var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
                // Make sure the velocity never goes past the required velocity provided by the developer
                rigidbody.AddForce((velocity - velocityProjectedToTarget) * acceleration, mode);
            }
        }

        public static void ApplyTorqueToReachRPS(Rigidbody rigidbody, Quaternion rotation, float rps, float force = 1)
        {
            var radPerSecond = rps * 2 * Mathf.PI + rigidbody.angularDrag * 20;

            float angleInDegrees;
            Vector3 rotationAxis;
            rotation.ToAngleAxis(out angleInDegrees, out rotationAxis);

            if (force == 0 || rotationAxis == Vector3.zero)
            {
                return;
            }

            rigidbody.maxAngularVelocity = Mathf.Max(rigidbody.maxAngularVelocity, radPerSecond);

            force = Mathf.Clamp(force, -rigidbody.mass * 2 * Mathf.PI / Time.fixedDeltaTime, rigidbody.mass * 2 * Mathf.PI / Time.fixedDeltaTime);

            var currentSpeed = Vector3.Project(rigidbody.angularVelocity, rotationAxis).magnitude;

            rigidbody.AddTorque(rotationAxis * (radPerSecond - currentSpeed) * force);
        }

        // Convert rotation in Unity into angular velocity
        public static Vector3 QuaternionToAngularVelocity(Quaternion rotation)
        {
            float angleInDegrees;
            Vector3 rotationAxis;
            rotation.ToAngleAxis(out angleInDegrees, out rotationAxis);
            return rotationAxis * angleInDegrees * Mathf.Deg2Rad;
        }

        // Convert angular velocity into rotation in Unity
        public static Quaternion AngularVelocityToQuaternion(Vector3 angularVelocity)
        {
            var rotationAxis = (angularVelocity * Mathf.Rad2Deg).normalized;
            float angleInDegrees = (angularVelocity * Mathf.Rad2Deg).magnitude;

            return Quaternion.AngleAxis(angleInDegrees, rotationAxis);
        }

        #region Incline Plane functions
        public static float GetRampSphericalVelocity(float inertiaCoefficient, float rampHeight)
        {
            float velocity = (Mathf.Abs(Physics.gravity.y) * rampHeight)
                / (0.5f + 0.5f * inertiaCoefficient);
            return (float)Mathf.Sqrt(velocity);
        }

        public static float GetRampSphericalAccel(float inertiaCoefficient, float rampHeight, float rampDistance)
        {
            float accel = ((Mathf.Abs(Physics.gravity.y)) * (rampHeight / rampDistance))
                / (2 * (0.5f + 0.5f * inertiaCoefficient));
            return (float)accel;
        }
        #endregion

        #region Helper Function
        public static Vector3 GetNormal(Vector3[] points)
        {
            if (points.Length < 3)
            {
                return Vector3.up;
            }

            var center = GetCenter(points);

            float xx = 0f, xy = 0f, xz = 0f, yy = 0f, yz = 0f, zz = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                var r = points[i] - center;
                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            var det_x = yy * zz - yz * yz;
            var det_y = xx * zz - xz * xz;
            var det_z = xx * yy - xy * xy;

            if (det_x > det_y && det_x > det_z)
            {
                return new Vector3(det_x, xz * yz - xy * zz, xy * yz - xz * yy).normalized;
            }
            if (det_y > det_z)
            {
                return new Vector3(xz * yz - xy * zz, det_y, xy * xz - yz * xx).normalized;
            }
            else
            {
                return new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, det_z).normalized;
            }
        }

        public static Vector3 GetCenter(Vector3[] points)
        {
            var center = Vector3.zero;
            for (int i = 0; i < points.Length; i++)
            {
                center += points[i] / points.Length;
            }
            return center;
        }
        #endregion
    }
}
