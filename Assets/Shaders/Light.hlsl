
void PixelLights_float(out half3 Out)
{
    int pixelLightCount = GetAdditionalLightsCount();
        half3 diffuseColor = half3(0,0,0);
        
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetAdditionalLight(i, WorldPos);
            half3 attenuatedLightColor = light.color * (light.distanceAttenuation);
            diffuseColor += attenuatedLightColor;
        }
        
        Out = diffuseColor;
}