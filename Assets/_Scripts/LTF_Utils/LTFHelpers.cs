using System.Collections.Generic;
using UnityEngine;

namespace LTF.Utils
{
    public static class LTFHelpers
    {
        /// <summary>
        /// Based on https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle 
        /// Adapted for unity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void KnuthShuffle<T>(this IList<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        public static void Move<T>(this T[] a, int from, int to)
        {
            var tmp = a[from];
            if (from < to)
                for (int i = from; i < to; i++)
                    a[i] = a[i + 1];
            else
                for (int i = from; i > to; i--)
                    a[i] = a[i - 1];

            a[to] = tmp;
        }

        public static void Move<T>(this List<T> l, int from, int to)
        {
            var tmp = l[from];
            if (from < to)
                for (int i = from; i < to; i++)
                    l[i] = l[i + 1];
            else
                for (int i = from; i > to; i--)
                    l[i] = l[i - 1];

            l[to] = tmp;
        }

        public static T PickRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T PickRandom<T>(this System.Collections.Generic.List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static char PickRandom(this string s)
        {
            return s[Random.Range(0, s.Length)];
        }

        public static int LayerMaskToLayer(this LayerMask layerMask)
        {
            return Mathf.RoundToInt(Mathf.Log(layerMask.value, 2f));
        }

        public static int CountTrue(params bool[] args)
        {
            int count = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i])
                    count++;
            }
            return count;
        }

        public static int CountFalse(params bool[] args)
        {
            int count = 0;
            for(int i = 0; i < args.Length; i++)
            {
                if (!args[i])
                    count++;
            }
            return count; 
        }

        #region Colour
        public static Color ChangeAlpha(this Color c, float a)
        {
            return new(c.r, c.g, c.b, a);
        }

        /// <summary>
        /// Combines two colours in a simple way
        /// Use CombineColoursSqrt for a more accurate colour
        /// </summary>
        public static Color CombineColour(Color c1, Color c2)
        {
            return new(
                (c1.r + c2.r) * .5f,
                (c1.g + c2.g) * .5f,
                (c1.b + c2.b) * .5f,
                (c1.a + c2.a) * .5f);
        }

        /// <summary>
        /// Combines multiple colours in a simple way
        /// Use CombineColoursSqrt for a more accurate colour
        /// </summary>
        public static Color CombineColours(params Color[] cs)
        {
            float r = 0f, g = 0f, b = 0f, a = 0f;
            for (int i = 0; i < cs.Length; i++)
            {
                r += cs[i].r;
                g += cs[i].g;
                b += cs[i].b;
                a += cs[i].a;
            }

            return new(r / cs.Length, g / cs.Length, b / cs.Length, a / cs.Length);
        }

        /// <summary>
        /// Combines multiple colours in a accurate way
        /// </summary>
        public static Color CombineColourSqrt(Color c1, Color c2)
        {
            return new(
                Mathf.Sqrt((c1.r + c2.r) * .5f),
                Mathf.Sqrt((c1.g + c2.g) * .5f),
                Mathf.Sqrt((c1.b + c2.b) * .5f),
                Mathf.Sqrt((c1.a + c2.a) * .5f));
        }

        /// <summary>
        /// Combines multiple colours in a accurate way
        /// </summary>
        public static Color CombineColourSqrt(params Color[] cs)
        {
            float r = 0f, g = 0f, b = 0f, a = 0f;
            for (int i = 0; i < cs.Length; i++)
            {
                r += cs[i].r;
                g += cs[i].g;
                b += cs[i].b;
                a += cs[i].a;
            }
            return new(Mathf.Sqrt(r / cs.Length),
                       Mathf.Sqrt(g / cs.Length),
                       Mathf.Sqrt(b / cs.Length),
                       Mathf.Sqrt(a / cs.Length));
        }
        #endregion
    }
}
