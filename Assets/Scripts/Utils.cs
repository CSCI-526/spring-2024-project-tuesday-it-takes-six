using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class Utils
    {
        /// <summary>
        /// When another game object (let's call it the collider) collides with the current game object,
        /// detect which side the collision happened
        /// </summary>
        /// <returns>
        /// Side
        /// </returns>
        public static Side DetectCollisionSide(Collision2D collision, Transform transform)
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 localNormal = transform.InverseTransformDirection(normal);
            float angle = Mathf.Atan2(localNormal.y, localNormal.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(angle) <= 45) return Side.LEFT;
            if (angle > 45 && angle <= 135) return Side.BOTTOM;
            if (angle < -45 && angle >= -135) return Side.TOP;
            return Side.RIGHT;
        }

        public static bool IsSegmentsIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersectPos)
        {
            intersectPos = Vector3.zero;

            Vector3 ab = b - a;
            Vector3 ca = a - c;
            Vector3 cd = d - c;

            Vector3 v1 = Vector3.Cross(ca, cd);

            if (Mathf.Abs(Vector3.Dot(v1, ab)) > 1e-6)
            {
                return false;
            }

            if (Vector3.Cross(ab, cd).sqrMagnitude <= 1e-6)
            {
                return false;
            }

            Vector3 ad = d - a;
            Vector3 cb = b - c;

            if (Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) || Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x)
                    || Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y) || Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y)
                    || Mathf.Min(a.z, b.z) > Mathf.Max(c.z, d.z) || Mathf.Max(a.z, b.z) < Mathf.Min(c.z, d.z)
               )
                return false;

            if (Vector3.Dot(Vector3.Cross(-ca, ab), Vector3.Cross(ab, ad)) > 0
                    && Vector3.Dot(Vector3.Cross(ca, cd), Vector3.Cross(cd, cb)) > 0)
            {
                Vector3 v2 = Vector3.Cross(cd, ab);
                float ratio = Vector3.Dot(v1, v2) / v2.sqrMagnitude;
                intersectPos = a + ab * ratio;
                return true;
            }

            return false;
        }

        public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
        }

        /// <summary>
        /// check if a rigidbody has velocity in y axis
        /// </summary>
        /// <returns>
        /// bool
        public static bool OnGround(Rigidbody2D rb)
        {
            return Mathf.Abs(rb.velocity.y) <= 1e-3;
        }
    }
}