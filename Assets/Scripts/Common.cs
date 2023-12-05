using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{
    public static float Frac(float v)
    {
        return v - Mathf.Floor(v);
    }
    public static Vector2 Frac(Vector2 v)
    {
        return new Vector2(v.x - Mathf.Floor(v.x), v.y - Mathf.Floor(v.y));
    }
    public static float Rd(Vector2 uv)
    {
        float dt = Vector2.Dot(uv, new Vector2(12.9898f, 78.233f));
        return Frac(Mathf.Sin(dt) * 43758.5453f);
    }

    public static float Noise(Vector2 uv)
    {
        Vector2 i = new Vector2(Mathf.Floor(uv.x), Mathf.Floor(uv.y));
        Vector2 f = Frac(uv);
        f = f * f * (new Vector2(3.0f, 3.0f) - 2.0f * f);

        uv = new Vector2(Mathf.Abs(Frac(uv.x) - 0.5f), Mathf.Abs(Frac(uv.y) - 0.5f));
        Vector2 c0 = i + new Vector2(0.0f, 0.0f);
        Vector2 c1 = i + new Vector2(1.0f, 0.0f);
        Vector2 c2 = i + new Vector2(0.0f, 1.0f);
        Vector2 c3 = i + new Vector2(1.0f, 1.0f);

        float r0 = Rd(c0);
        float r1 = Rd(c1);
        float r2 = Rd(c2);
        float r3 = Rd(c3);

        float t = Mathf.Lerp(Mathf.Lerp(r0, r1, f.x), Mathf.Lerp(r2, r3, f.x), f.y);
        return t;
    }

    public static float Noise(Vector2 uv, float scale)
    {
        float t = 0.0f;
        float freq = Mathf.Pow(2.0f, 0);
        float amp = Mathf.Pow(0.5f, 3);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 1);
        amp = Mathf.Pow(0.5f, 3 - 1);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 2);
        amp = Mathf.Pow(0.5f, 3 - 2);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;
        return t;
    }

}
