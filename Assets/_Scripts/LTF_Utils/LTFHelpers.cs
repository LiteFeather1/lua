﻿using UnityEngine;

namespace LTF.Utils
{
    public static class LTFHelpers
    {
        public static void KnuthShuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
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

        public static Color CombineColour(Color c1, Color c2)
        {
            return new(
                (c1.r + c2.r) * .5f,
                (c1.g + c2.g) * .5f,
                (c1.b + c2.b) * .5f,
                (c1.a + c2.a) * .5f);
        }

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

        public static Color CombineColourSqrt(Color c1, Color c2)
        {
            return new(
                Mathf.Sqrt((c1.r + c2.r) * .5f),
                Mathf.Sqrt((c1.g + c2.g) * .5f),
                Mathf.Sqrt((c1.b + c2.b) * .5f),
                Mathf.Sqrt((c1.a + c2.a) * .5f));
        }

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