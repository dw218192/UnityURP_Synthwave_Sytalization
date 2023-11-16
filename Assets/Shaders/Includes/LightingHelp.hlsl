void GetMainLight_float(float3 WorldPos, out float3 Color, out float3 Direction, out float DistanceAtten, out float ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(0.5, 0.5, 0));
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
#if SHADOWS_SCREEN
        float4 clipPos = TransformWorldToClip(WorldPos);
        float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}

void ComputeAdditionalLighting_float(float3 WorldPosition, float3 WorldNormal,
    float2 Thresholds, float3 RampedDiffuseValues,
    out float3 Color, out float Diffuse)
{
    Color = float3(0, 0, 0);
    Diffuse = 0;

#ifndef SHADERGRAPH_PREVIEW

    int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPosition);
        float4 tmp = unity_LightIndices[i / 4];
        uint light_i = tmp[i % 4];

        half shadowAtten = light.shadowAttenuation * AdditionalLightRealtimeShadow(light_i, WorldPosition, light.direction);

        half NdotL = saturate(dot(WorldNormal, light.direction));
        half distanceAtten = light.distanceAttenuation;

        half thisDiffuse = distanceAtten * shadowAtten * NdotL;

        half rampedDiffuse = 0;

        if (thisDiffuse < Thresholds.x)
        {
            rampedDiffuse = RampedDiffuseValues.x;
        }
        else if (thisDiffuse < Thresholds.y)
        {
            rampedDiffuse = RampedDiffuseValues.y;
        }
        else
        {
            rampedDiffuse = RampedDiffuseValues.z;
        }


        if (shadowAtten * NdotL == 0)
        {
            rampedDiffuse = 0;

        }

        if (light.distanceAttenuation <= 0)
        {
            rampedDiffuse = 0.0;
        }

        Color += max(rampedDiffuse, 0) * light.color.rgb;
        Diffuse += rampedDiffuse;
    }
#endif
}

void ChooseColor_float(float3 Highlight, float3 Midtone, float3 Shadow, float Diffuse, float2 Thresholds, out float3 OUT)
{
    if (Diffuse < Thresholds.x)
    {
        OUT = Shadow;
    }
    else if (Diffuse < Thresholds.y)
    {
        OUT = Midtone;
    }
    else
    {
        OUT = Highlight;
    }
}

float BlinnPhongOneLight(float3 normal, float3 viewDir, float3 worldPos, float3 lightColor, float3 lightDir, float lightDistanceAtten, float lightShadowAtten, float specWeight, float shiness, float rimWeight)
{
    float3 reflectDir = reflect(-lightDir, normal);
    // Compute the specular reflection using the Blinn-Phong model
    float3 halfVec = normalize(lightDir + viewDir);
    float specular = pow(max(dot(halfVec, normal), 0.0), shiness);
    float rim = max(0, abs(dot(normal, viewDir)));
    
    float lightDotNormal = saturate(dot(normal, lightDir));
    float shaded = lightDotNormal * lightColor * lightDistanceAtten * lightShadowAtten + 
        specWeight * specular + rim * rimWeight;
    return shaded;
}


void BlinnPhong_float(
    float3 ambientColor,
    float ambientWeight,
    float3 normal,
    float3 viewDir,
    float3 worldPos,
    float specWeight,
    float shiness,
    float rimWeight,
    out float shaded
) {
    float3 mainLightColor;
    float3 mainLightDir;
    float mainLightDistanceAtten;
    float mainLightShadowAtten;

    GetMainLight_float(worldPos, mainLightColor, mainLightDir, mainLightDistanceAtten, mainLightShadowAtten);
    shaded = BlinnPhongOneLight(normal, viewDir, worldPos, mainLightColor, mainLightDir, mainLightDistanceAtten, mainLightShadowAtten, specWeight, shiness, rimWeight);
    
#ifndef SHADERGRAPH_PREVIEW
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i) {
        Light light = GetAdditionalLight(i, worldPos);
        float4 tmp = unity_LightIndices[i / 4];
        uint light_i = tmp[i % 4];
        float3 lightColor = light.color.rgb;
        float3 lightDir = light.direction;
        float lightDistanceAtten = light.distanceAttenuation;
        float lightShadowAtten = light.shadowAttenuation * AdditionalLightRealtimeShadow(light_i, worldPos, lightDir);
        shaded += BlinnPhongOneLight(normal, viewDir, worldPos, lightColor, lightDir, lightDistanceAtten, lightShadowAtten, specWeight, shiness, rimWeight);
    }
#endif
    shaded += ambientColor * ambientWeight;
}

void BlendSamples_float(
    Texture2DArray texs,
    SamplerState samp,
    float2 uv,
    float intensity,
    float uv_scale,
    float specFactor,
    out float3 sampled_color
) {
    // intensity = clamp(intensity, 0, 1);
    
    int w,h,n;
    texs.GetDimensions(w, h, n);
    float val = intensity * n;
    int tex1 = floor(val);
    int tex2 = ceil(val);
    float fr = frac(val);
    float2 uv2 = uv * uv_scale;
    float3 color1 = texs.SampleLevel(samp, float3(uv2, tex1), 0);
    float3 color2 = tex2 == n ? float3(1, 1, 1) : texs.SampleLevel(samp, float3(uv2, tex2), 0);
    sampled_color = lerp(color1, color2, fr);
}

void Stepify_float(float val, float min_val, float max_val, float step, out float out_val) {
    float range = max_val - min_val;
    float num_steps = range / step;
    float step_size = range / num_steps;
    float step_val = floor((val - min_val) / step_size);
    out_val = min_val + step_val * step_size;
}

float3 convertAdjustColorLevel(float3 colorLevel) {
    float scale = 0.01;
    float redAdjustment = -colorLevel.x * scale; 
    float greenAdjustment = -colorLevel.y * scale; 
    float blueAdjustment = colorLevel.z * scale; 

    return float3(redAdjustment, greenAdjustment, blueAdjustment);
}

float3 AdjustColorBalance(float3 color, float3 shadowsAdjustments, float3 midtonesAdjustments, float3 highlightsAdjustments)
{
    float luminance = dot(color, float3(0.2126, 0.7152, 0.0722));
    float3 shadows = saturate((0.5 - luminance) * 2.0);
    float3 highlights = saturate((luminance - 0.5) * 2.0);
    float3 midtones = 1.0 - shadows - highlights;

    color += shadowsAdjustments * shadows;
    color += midtonesAdjustments * midtones;
    color += highlightsAdjustments * highlights;

    return saturate(color);
}

#define PI 3.14159265359
#define EPS 0.00001

float fPow5(float v)
{
    return pow(1 - v, 5);
}

// Diffuse distribution functions

float3 lambertDiffuse(float3 albedo)
{
    return albedo / PI;
}

// Fresnel functions

float3 fresnel(float3 F0, float NdotV)
{
    return F0 + (1 - F0) * fPow5(NdotV);
}

float3 fresnel(float3 F0, float NdotV, float roughness)
{
    return F0 + (max(1.0 - roughness, F0) - F0) * fPow5(NdotV);
}

float3 fresnelDisney(float HdotL, float NdotL, float NdotV, float roughness)
{
    float k = 0.5 + 2 * roughness * sqrt(HdotL);
    float firstTerm = (k - 1) * fPow5(NdotV) + 1;
    float secondTerm = (k - 1) * fPow5(NdotL) + 1;
    return firstTerm * secondTerm;
}

float3 F0(float ior)
{
    return pow((1.0 - ior) / (1.0 + ior), 2);
}

// Normal distribution functions

float trowbridgeReitzNDF(float NdotH, float roughness)
{
    float alpha = roughness * roughness;
    float alpha2 = alpha * alpha;
    float NdotH2 = NdotH * NdotH;
    float denominator = PI * pow((alpha2 - 1) * NdotH2 + 1, 2);
    return alpha2 / denominator;
}

float trowbridgeReitzAnisotropicNDF(float NdotH, float roughness, float anisotropy, float HdotT, float HdotB)
{
    float aspect = sqrt(1.0 - 0.9 * anisotropy);
    float alpha = roughness * roughness;

    float roughT = alpha / aspect;
    float roughB = alpha * aspect;

    float alpha2 = alpha * alpha;
    float NdotH2 = NdotH * NdotH;
    float HdotT2 = HdotT * HdotT;
    float HdotB2 = HdotB * HdotB;

    float denominator = PI * roughT * roughB * pow(HdotT2 / (roughT * roughT) + HdotB2 / (roughB * roughB) + NdotH2, 2);
    return 1 / denominator;
}

// Geometric attenuation functions

float cookTorranceGAF(float NdotH, float NdotV, float HdotV, float NdotL)
{
    float firstTerm = 2 * NdotH * NdotV / HdotV;
    float secondTerm = 2 * NdotH * NdotL / HdotV;
    return min(1, min(firstTerm, secondTerm));
}

float schlickBeckmannGAF(float dotProduct, float roughness)
{
    float alpha = roughness * roughness;
    float k = alpha * 0.797884560803;  // sqrt(2 / PI)
    return dotProduct / (dotProduct * (1 - k) + k);
}

// Helpers
float3 gammaCorrection(float3 v)
{
    return pow(v, 1.0 / 2.2);
}

float3 sRGB2Lin(float3 col)
{
    return pow(col, 2.2);
}

float3 CalculatePBRColor(float3 normal, float3 viewVec, float3 lightVec, float3 albedo, float roughness, float metalness)
{
    float3 halfVec = normalize(lightVec + viewVec);
    float NdotL = max(dot(normal, lightVec), 0.0);
    float NdotH = max(dot(normal, halfVec), 0.0);
    float NdotV = max(dot(normal, viewVec), 0.0);

    float3 F0 = lerp(float3(0.04, 0.04, 0.04), albedo, metalness);
    float D = trowbridgeReitzNDF(NdotH, roughness);
    float3 F = fresnel(F0, NdotV, roughness);
    float G = schlickBeckmannGAF(NdotV, roughness) * schlickBeckmannGAF(NdotL, roughness);

    float3 brdfOutput = (D * F * G) / (4 * NdotV * NdotL + 0.001);

    return brdfOutput;
}

float3 PBR(float3 worldPos, float3 normal, float3 viewVec, float3 albedo, float roughness, float metalness)
{
    float3 mainLightColor;
    float3 mainLightDir;
    float mainLightDistanceAtten;
    float mainLightShadowAtten;

    GetMainLight_float(worldPos, mainLightColor, mainLightDir, mainLightDistanceAtten, mainLightShadowAtten);
    float3 brdfOutput = CalculatePBRColor(normal, viewVec, mainLightDir, albedo * 100.f, roughness, metalness);
    return brdfOutput * mainLightColor;
}

void Postprocess_float(float3 worldPos, float3 normal, float3 viewVec, float3 albedo, float roughness, float metalness,
    float3 Shadows, float3 Midtones, float3 Highlights,
    out float3 OUT)
{
    float3 mainLightColor;
    float3 mainLightDir;
    float mainLightDistanceAtten;
    float mainLightShadowAtten;

    GetMainLight_float(worldPos, mainLightColor, mainLightDir, mainLightDistanceAtten, mainLightShadowAtten);
    const float3 col = BlinnPhongOneLight(normal, viewVec, worldPos, mainLightColor, mainLightDir, mainLightDistanceAtten, mainLightShadowAtten, 0, 0, 0);
    // OUT = AdjustColorBalance(pbrCol, convertAdjustColorLevel(Shadows), convertAdjustColorLevel(Midtones), convertAdjustColorLevel(Highlights));
    OUT = AdjustColorBalance(saturate(col), convertAdjustColorLevel(Shadows), convertAdjustColorLevel(Midtones), convertAdjustColorLevel(Highlights));
}