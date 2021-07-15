using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class MinimapCamera : MonoBehaviour
{
    [SerializeField]private bool isRotate = false;
    [SerializeField] private float stageOneSize;
    [SerializeField] private float stageTwoSize;
    [SerializeField] private float normalSize;
    [SerializeField] private Camera mapCamera;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] Vector3 newPos;
    [ReadOnly] [SerializeField] Vector3 oldPos;

    private void OnEnable()
    {
        oldPos = transform.position;

        GameManager.Instance.mapStateChange += MapState;

        SetMapCameraSize();
    }

    private void OnDisable()
    {
        GameManager.Instance.mapStateChange -= MapState;
    }

    private void MapState(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetMapState)
        {
            transform.position = GameManager.Instance.overviewMap.transform.position;
        }
        else
        {
            transform.position = oldPos;
        }

        SetMapCameraSize();
    }

    private void SetMapCameraSize()
    {
        if (GameManager.Instance.GetSetMapState)
        {
            if (SceneManager.GetActiveScene().name == "StageOne")
                mapCamera.orthographicSize = stageOneSize;
            else if (SceneManager.GetActiveScene().name == "StageTwo")
                mapCamera.orthographicSize = stageTwoSize;
        }
        else
        {
            mapCamera.orthographicSize = normalSize;
        }
    }

    void LateUpdate()
    {
        if (GameManager.Instance.playerObj != null && !GameManager.Instance.GetSetMapState)
        {
            newPos = GameManager.Instance.playerObj.transform.position;
            newPos.y = transform.position.y;
            transform.position = newPos;

            if (isRotate)
                transform.rotation = Quaternion.Euler(90f, GameManager.Instance.playerObj.transform.eulerAngles.y, 0f);
            else
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
