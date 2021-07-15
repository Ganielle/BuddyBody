using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIndicatorAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;

    private void OnEnable()
    {
        GameManager.Instance.sceneController.onLoadingChange += LoadingChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.sceneController.onLoadingChange -= LoadingChange;
    }

    private void LoadingChange(object sender, EventArgs e)
    {
        CheckIfChangeScene();
    }

    private void CheckIfChangeScene()
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus != SceneController.LoadingStatus.NONE)
            return;

        parentObj.SetActive(false);
    }

    public void DeactivateObject() => parentObj.SetActive(false);
}
