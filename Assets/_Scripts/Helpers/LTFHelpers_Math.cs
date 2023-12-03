using UnityEngine;

namespace LTFUtils
{
    public static class LTFHelpers_Math
    {
        /// <summary>
        /// Map a value from a range to another
        /// </summary>
        public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
        {
            float map = min2 + (max2 - min1) * ((value - min1) / (max1 - min1));
            return clamp ? Mathf.Clamp(map, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : map;
        }

        /// <summary>
        /// Returns the angle between two points
        /// </summary>
        public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {                   
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle between two points
        /// </summary>
        public static float AngleBetweenTwoPoints(Vector3 a, Vector2 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle between two points
        /// </summary>
        public static float AngleBetweenTwoPoints(Vector2 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle between two points
        /// </summary>
        public static float AngleBetweenTwoPoints(Vector2 a, Vector2 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public static Vector2 RotateVector(this Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
            float y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
            return new(x, y);
        }

        public static bool CompareQuartertions(Quaternion quatA, Quaternion quatB, float range)
        {
            return Quaternion.Angle(quatA, quatB) < range;
        }
    }
}
