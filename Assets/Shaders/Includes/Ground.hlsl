void shade_float(float2 uv, float2 sz,  float bloom, float3 dark, float3 bright, out float3 col) {
    uv = abs(frac(uv) - 0.5);
    float2 lines = smoothstep(sz, float2(0,0), uv);
    lines += smoothstep(sz, float2(0,0), uv) * 0.4 * bloom;
    col = lerp(dark, bright, clamp(lines.x + lines.y, 0.0, 3.0));
}