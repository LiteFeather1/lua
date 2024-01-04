using UnityEngine;

namespace LTF.Utils
{
    public static class LTFHelpersMath
    {
        public static float Evaluate(this Vector2 v2, float t)
        {
            return v2.x + (v2.y - v2.x) * t;
        }

        public static float EvaluateClamped(this Vector2 v2, float t)
        {
            if (t > 1f)
                t = 1f;
            else if (t < 0f)
                t = 0f;

            return v2.x + (v2.y - v2.x) * t;
        }

        public static int Evaluate(this Vector2Int v2, float t)
        {
            return (int)(v2.x + (v2.y - v2.x) * t);
        }

        public static float Evaluate(this Vector4 v4, float t)
        {
            float min = v4.x + (v4.y - v4.x) * t;
            float max = v4.z + (v4.w - v4.z) * t;
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(this Vector2 v2)
        {
            return UnityEngine.Random.value * (v2.y - v2.x) + v2.x;
        }

        public static int RandomExclusive(this Vector2Int v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y);
        }

        public static int RandomInclusive(this Vector2Int v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y + 1);
        }

        public static float BiasedRandom(float rng, float mixRng, float min, float max, float bias, float influence)
        {
            float r = rng * (max - min) + min;
            float mix = mixRng * influence;
            return r * (1f - mix) + bias * mix;
        }

        public static float BiasedRandom(float min, float max, float bias, float influence)
        {
            float r = UnityEngine.Random.value * (max - min) + min;
            float mix = UnityEngine.Random.value * influence;
            return r * (1f - mix) + bias * mix;
        }

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

        public static Vector2 RotateVector(this Vector3 v, float angle)
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
