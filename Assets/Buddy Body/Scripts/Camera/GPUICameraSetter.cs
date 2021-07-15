using GPUInstancer;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUICameraSetter : MonoBehaviour
{
    [Header("Booleans")]
    [SerializeField] private bool prefabManager;
    [SerializeField] private bool detailManager;
    [SerializeField] private bool treeManager;

    [Header("GPUI Managers")]
    [ConditionalField("prefabManager")] [SerializeField] private GPUInstancerPrefabManager gpuiPrefab;
    [ConditionalField("detailManager")] [SerializeField] private GPUInstancerDetailManager gpuiDetail;
    [ConditionalField("treeManager")] [SerializeField] private GPUInstancerTreeManager gpuiTree;

    private void Awake()
    {
        if (prefabManager)
            gpuiPrefab.SetCamera(GameManager.Instance.mainCamera);

        if (detailManager)
            gpuiDetail.SetCamera(GameManager.Instance.mainCamera);

        if (treeManager)
            gpuiTree.SetCamera(GameManager.Instance.mainCamera);
    }
}
