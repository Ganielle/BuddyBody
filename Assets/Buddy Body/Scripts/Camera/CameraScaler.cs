using Cinemachine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScaler : MonoBehaviour 
{
    //[SerializeField] float leftRatio = 16.0f, rightRatio = 9.0f;
    public float horizontalFoV = 90.0f;
    [SerializeField] private bool isCamera;
    [ConditionalField("isCamera", true)] [SerializeField] CinemachineFreeLook vcam;
    [ConditionalField("isCamera")][SerializeField] Camera cameraPlayer;
    [SerializeField] private bool isOrthographicCamera, debugging;

    [ConditionalField("debugging")] [SerializeField] private TextMeshProUGUI debugger;

    //float targetaspect;
    //float windowaspect;

    //float scaleheight;

    int width;
    int height;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float halfwidth;
    [ReadOnly] [SerializeField] float halfheight;
    [ReadOnly] [SerializeField] float verticalFOV;


    // Use this for initialization
    void Awake ()
    {
        if (isCamera)
        {
            if (isOrthographicCamera)
                cameraPlayer.orthographic = true;
            else
                cameraPlayer.orthographic = false;
        }

        ScaleWithScreenSize();
    }

    private void Update()
    {
        if ((Screen.width != width || Screen.height != height) && 
            (Input.deviceOrientation == DeviceOrientation. LandscapeLeft|| 
            Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
            Input.deviceOrientation == DeviceOrientation.Unknown))
            ScaleWithScreenSize();
    }

    private void ScaleWithScreenSize()
    {
        width = Screen.width;
        height = Screen.height;

        halfwidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);

        halfheight = halfwidth * Screen.height / Screen.width;

        verticalFOV = 2.0f * Mathf.Atan(halfheight) * Mathf.Rad2Deg;

        if (isCamera)
            cameraPlayer.fieldOfView = verticalFOV;
        else
            vcam.m_Lens.FieldOfView = verticalFOV;


        //targetaspect = leftRatio / rightRatio;

        //if (debugging)
        //    StartCoroutine(ChangeScreen());

        //windowaspect = (float)Screen.width / (float)Screen.height;
        //scaleheight = windowaspect / targetaspect;

        //if (scaleheight < 1.0f)
        //{
        //    Rect rect = cameraPlayer.rect;

        //    rect.width = 1.0f;
        //    rect.height = scaleheight;
        //    rect.x = 0;
        //    rect.y = (1.0f - scaleheight) / 2.0f;

        //    cameraPlayer.rect = rect;
        //}
        //else // add pillarbox
        //{
        //    float scalewidth = 1.0f / scaleheight;

        //    Rect rect = cameraPlayer.rect;

        //    rect.width = scalewidth;
        //    rect.height = 1.0f;
        //    rect.x = (1.0f - scalewidth) / 2.0f;
        //    rect.y = 0;

        //    cameraPlayer.rect = rect;
        //}
    }

    //IEnumerator ChangeScreen()
    //{
    //    debugger.text = "Change screen";
    //    yield return new WaitForSecondsRealtime(5f);

    //    debugger.text = "";
    //}
}
