using UnityEngine;

public static class Extentions
{
    public static float Evaluate(this Vector2 v2, float t)
    {
        return v2.x + (v2.y - v2.x) * t;
    }

    public static int Evaluate(this Vector2Int v2, float t)
    {
        return (int)(v2.x + (v2.y - v2.x) * t);
    }


    public static float Random(this Vector2 v2)
    {
        return UnityEngine.Random.Range(v2.x, v2.y);
    }

    public static float Random(this Vector2Int v2)
    {
        return UnityEngine.Random.Range(v2.x, v2.y);
    }
}
