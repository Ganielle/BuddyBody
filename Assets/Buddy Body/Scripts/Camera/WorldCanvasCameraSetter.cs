using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasCameraSetter : MonoBehaviour
{
    [SerializeField] private Canvas canvasWorld;

    private void OnEnable()
    {
        canvasWorld.worldCamera = GameManager.Instance.mainCamera;
    }
}
