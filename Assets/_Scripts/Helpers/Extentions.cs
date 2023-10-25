using UnityEngine;

public static class Extentions
{
    public static float Evaluate(this Vector2 v2, float t)
    {
        return v2.x + (v2.y - v2.x) * t;
    }
}
