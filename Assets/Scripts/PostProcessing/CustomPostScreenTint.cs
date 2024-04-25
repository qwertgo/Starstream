using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/ScreenTint", typeof(UniversalRenderPipeline))]
public class CustomPostScreenTint : VolumeComponent, IPostProcessComponent
{
    public FloatParameter tintIntesity = new FloatParameter(1);
    public ColorParameter color = new ColorParameter(Color.white);

    public bool IsActive() => true;

    public bool IsTileCompatible() => true;
}
