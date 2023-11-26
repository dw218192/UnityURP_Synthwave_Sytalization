void shade_float(float2 uv, float2 sz, float bloom, float3 dark, float3 bright, out float3 col) {
    uv = abs(frac(uv) - 0.5);
    float2 lines = smoothstep(sz, float2(0,0), uv);
    lines += smoothstep(sz, float2(0,0), uv) * 0.4 * bloom;
    col = lerp(dark, bright, clamp(lines.x + lines.y, 0.0, 3.0));
}
void get_brightness_float(float3 pos, float x_max, float3 dark, float3 bright, out float3 col) {
    float t = abs(pos.x) / x_max;
    col = lerp(dark, bright, sqrt(t));
}

inline float rd (float2 uv) {
    return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
}
inline float noise (float2 uv) {
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = rd(c0);
    float r1 = rd(c1);
    float r2 = rd(c2);
    float r3 = rd(c3);
    float t = lerp(lerp(r0, r1, f.x), lerp(r2, r3, f.x), f.y);
    return t;
}
inline float noise(float2 uv, float scale) {
    float t = 0.0;
    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3));
    t += noise(float2(uv.x*scale/freq, uv.y*scale/freq))*amp;

    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3-1));
    t += noise(float2(uv.x*scale/freq, uv.y*scale/freq))*amp;

    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3-2));
    t += noise(float2(uv.x*scale/freq, uv.y*scale/freq))*amp;
    return t;
}
// From https://www.shadertoy.com/view/WttXWX
inline uint hash(uint x) {
    x ^= x >> 16;
    x *= 0x7feb352dU;
    x ^= x >> 15;
    x *= 0x846ca68bU;
    x ^= x >> 16;
    return x;
}
inline float hash(float2 vf) {
    uint offset = vf.x < 0.0 ? 13u : 0u;
    uint2 vi = uint2(abs(vf));
    return float(hash(vi.x + (vi.y<<16) + offset)) / float( 0xffffffffU );
}
void vertex_height_float(float3 pos, float noise_freq, float hi, float speed, float xthres, float xmax, out float3 outpos) {
    float2 sp = float2(pos.x, pos.z - speed * _Time.x);
    float offset = 0.0;
    if (abs(pos.x) >= xthres) {
        // smooth step between 0 and hi
        float t = (abs(pos.x) - xthres) / (xmax - xthres); // [0,1]
        pos.y += hi * smoothstep(0, 1, t) + noise(sp * noise_freq, 2.2);
    } else {
        pos.y += noise(sp * noise_freq) * 0.1;
    }
    
    outpos = pos;
}