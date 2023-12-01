using UnityEngine;

public static class Extentions
{
    public static float Evaluate(this Vector2 v2, float t)
    {
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
}
