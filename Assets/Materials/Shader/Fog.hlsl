#ifndef FOG_INCLUDED
#define FOG_INCLUDED

void GetFog_float(float2 UV, out float3 CameraPos){
    CameraPos = _WorldSpaceCameraPos;
}

#endif