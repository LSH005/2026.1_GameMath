using UnityEngine;

public class ThatPerlin : MonoBehaviour
{
    public static int seed = 0;

    static Vector2[] gradients =
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(1, 1).normalized,
        new Vector2(-1, -1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized
    };

    static Vector2 GetGradient(int x, int z)
    {
        int hash = x * 6581963 + z * 7368787 + seed;
        int index = Mathf.Abs(hash % gradients.Length);
        return gradients[index];
    }

    static float Fade(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float Noise(float x, float z)
    {
        int x0 = Mathf.FloorToInt(x);
        int z0 = Mathf.FloorToInt(z);
        int x1 = x0 + 1;
        int z1 = z0 + 1;

        float u = x - x0;
        float v = z - z0;

        Vector2 g00 = GetGradient(x0, z0);
        Vector2 g10 = GetGradient(x1, z0);
        Vector2 g01 = GetGradient(x0, z1);
        Vector2 g11 = GetGradient(x1, z1);

        Vector2 d00 = new Vector2(u, v);
        Vector2 d10 = new Vector2(u - 1, v);
        Vector2 d01 = new Vector2(u, v - 1);
        Vector2 d11 = new Vector2(u - 1, v - 1);

        float s00 = Dot(g00, d00);
        float s10 = Dot(g10, d10);
        float s01 = Dot(g01, d01);
        float s11 = Dot(g11, d11);

        float fu = Fade(u);
        float fv = Fade(v);

        float nx0 = Lerp(s00, s10, fu);
        float nx1 = Lerp(s01, s11, fu);

        float value = Lerp(nx0, nx1, fv);

        //return value + 1 / 2;
        //return value * 0.5f + 0.5f;
        return 0.5f * (value + 1f);
    }
}
