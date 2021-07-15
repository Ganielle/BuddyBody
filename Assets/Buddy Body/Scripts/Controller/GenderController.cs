using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderController : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject maleMesh;
    [SerializeField] private GameObject femaleMesh;

    [Header("Animator")]
    [SerializeField] private Animator maleAnimator;
    [SerializeField] private Animator femaleAnimator;

    GenderSetter genderSetter;

    private void OnEnable()
    {
        GameManager.Instance.playerObj = gameObject;

        genderSetter = GameManager.Instance.genderSetter;

        genderSetter.onChangeGender += ChangeGender;
        GameManager.Instance.sceneController.onLoadingChange += ChangeGender;

        GenderAnimatorSetter();
        GenderMeshEnabler();
    }

    private void OnDisable()
    {
        genderSetter.onChangeGender -= ChangeGender;
        GameManager.Instance.sceneController.onLoadingChange -= ChangeGender;

        genderSetter = null;
    }

    private void ChangeGender(object sender, EventArgs e)
    {
        GenderAnimatorSetter();
        GenderMeshEnabler();
    }

    private void GenderAnimatorSetter()
    {
        if (genderSetter.GetSetGender == GenderSetter.Gender.MALE)
            genderSetter.GetSetGenderAnimator = maleAnimator;

        else if (genderSetter.GetSetGender == GenderSetter.Gender.FEMALE)
            genderSetter.GetSetGenderAnimator = femaleAnimator;
    }

    private void GenderMeshEnabler()
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE ||
            GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.FINISHLOADING)
        {
            maleMesh.SetActive(false);
            femaleMesh.SetActive(false);

            if (genderSetter.GetSetGender == GenderSetter.Gender.MALE)
                maleMesh.SetActive(true);

            else if (genderSetter.GetSetGender == GenderSetter.Gender.FEMALE)
                femaleMesh.SetActive(true);
        }
    }
}
