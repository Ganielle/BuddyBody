using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;
using System;

public class PlayerCameraObjSetter : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook playerCameraCM;
    [SerializeField] private SimpleFollowRecenter simpleFollowRecenter;

    [Header("Settings")]
    [ReadOnly] [SerializeField] bool doneStart;

    private void OnEnable()
    {
        doneStart = false;

        GameManager.Instance.onTutorialStateChange += TutorialStateChange;

        if (doneStart)
        {
            playerCameraCM.m_LookAt = GameManager.Instance.playerObj.transform;
            playerCameraCM.m_Follow = GameManager.Instance.playerObj.transform;

            StartCoroutine(RecenterCinemachinePlayerCamera(0.5f, 0f));
        }
    }

    private void OnDisable()
    {
        playerCameraCM.m_LookAt = null;
        playerCameraCM.m_Follow = null;

        GameManager.Instance.onTutorialStateChange -= TutorialStateChange;
    }

    private void Start()
    {
        playerCameraCM.m_LookAt = GameManager.Instance.playerObj.transform;
        playerCameraCM.m_Follow = GameManager.Instance.playerObj.transform;

        StartCoroutine(RecenterCinemachinePlayerCamera(0.5f, 0f));

        doneStart = true;
    }

    private void TutorialStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.NONE)
            playerCameraCM.enabled = true;
        else
            playerCameraCM.enabled = false;
    }

    IEnumerator RecenterCinemachinePlayerCamera(float delay, float recenteringTime)
    {
        playerCameraCM.m_RecenterToTargetHeading.m_enabled = true;
        simpleFollowRecenter.recenter = true;
        playerCameraCM.m_YAxisRecentering.m_RecenteringTime = recenteringTime;
        simpleFollowRecenter.recenterTime = recenteringTime;

        yield return new WaitForSeconds(delay);

        playerCameraCM.m_RecenterToTargetHeading.m_enabled = false;
        simpleFollowRecenter.recenter = false;
    }
}
