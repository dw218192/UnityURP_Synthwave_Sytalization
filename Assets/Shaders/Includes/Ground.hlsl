void shade_float(float2 uv, float2 sz,  float bloom, float3 dark, float3 bright, out float3 col) {
    uv = abs(frac(uv) - 0.5);
    float2 lines = smoothstep(sz, float2(0,0), uv);
    lines += smoothstep(sz, float2(0,0), uv) * 0.4 * bloom;
    col = lerp(dark, bright, clamp(lines.x + lines.y, 0.0, 3.0));
}

// From https://www.shadertoy.com/view/WttXWX
uint hash(uint x) {
    x ^= x >> 16;
    x *= 0x7feb352dU;
    x ^= x >> 15;
    x *= 0x846ca68bU;
    x ^= x >> 16;
    return x;
}

float hash(float2 vf) {
    uint offset = vf.x < 0.0 ? 13u : 0u;
    uint2 vi = uint2(abs(vf));
    return float(hash(vi.x + (vi.y<<16) + offset)) / float(0xffffffffU);
}

void vertex_height_float(float3 pos, float hi, float xthres, float xmax, out float3 outpos) {
    float offset = 0.0;
    if (abs(pos.x) >= xthres) {
        // smooth step between 0 and hi
        float t = (abs(pos.x) - xthres) / (xmax - xthres); // [0,1]
        t *= hi; // [0,hi]
        pos.y += smoothstep(0, hi, t) + hash(pos.xz * 4.0) * 0.4;
    } else {
        pos.y += hash(pos.xz * 4.0) * 0.1;
    }
    
    outpos = pos;
}