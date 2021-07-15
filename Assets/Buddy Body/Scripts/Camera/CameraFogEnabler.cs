using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class CameraFogEnabler : MonoBehaviour
{
    public bool AllowFog = false;

    private bool FogOn;

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_startCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_startCameraRendering;
    }

    private void RenderPipelineManager_startCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        OnPreRender();
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        OnPostRender();
    }

    private void OnPreRender()
    {
        FogOn = RenderSettings.fog;
        RenderSettings.fog = AllowFog;
    }

    private void OnPostRender()
    {
        RenderSettings.fog = FogOn;
    }
}
