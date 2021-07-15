using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettingSetter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Light directionalLight;
    [SerializeField] private Material skybox;

    private void Start()
    {
        RenderSettings.skybox = skybox;
        RenderSettings.sun = directionalLight;
    }
}
