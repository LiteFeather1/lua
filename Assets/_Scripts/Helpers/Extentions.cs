using UnityEngine;

namespace LTFUtils
{
    public static class Extentions
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

        public static float Evaluate(this Vector4 v4, float t)
        {
            float min = v4.x + (v4.y - v4.x) * t;
            float max = v4.z + (v4.w - v4.z) * t;
            return UnityEngine.Random.Range(min, max);
        }

        public static int Evaluate(this Vector2Int v2, float t)
        {
            return (int)(v2.x + (v2.y - v2.x) * t);
        }

        public static float Random(this Vector2 v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y);
        }

        public static int Random(this Vector2Int v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y);
        }

        public static void KnuthShuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        public static T PickRandom<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T PickRandom<T>(this System.Collections.Generic.List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static char PickRandom(this string s)
        {
            return s[UnityEngine.Random.Range(0, s.Length)];
        }
    }
}
