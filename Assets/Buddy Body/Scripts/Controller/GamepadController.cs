using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float nextGrabButtonDelay;
    [SerializeField] private GameObject startQuestIndicator;
    [SerializeField] private GameObject finishQuestIndicator;
    [SerializeField] private GameObject bodyPartsAllFoundIndicator;
    [SerializeField] private GameObject timerObj;

    [Header("UI")]
    [SerializeField] private Button grabButton;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupItemClip;
    [SerializeField] private AudioClip openPauseMenuClip;

    InventoryStageOne inventoryStageOne;
    GenderSetter genderSetter;

    [Header("Debugging")]
    //  Analog
    [ReadOnly] [SerializeField] Vector3 direction;
    //  Button
    [ReadOnly] [SerializeField] bool canPickup;

    private void OnEnable()
    {
        inventoryStageOne = GameManager.Instance.inventoryStageOne;
        genderSetter = GameManager.Instance.genderSetter;
        GameManager.Instance.onActivateQuestChange += ShowQuestStartIndicator;

        inventoryStageOne.onBodyPartChange += GrabButtonEnabler;

        StartCoroutine(TimerEnabler());
        GrabButtonBodyPartChecker();
        canPickup = true;
    }

    private void OnDisable()
    {
        inventoryStageOne.onBodyPartChange -= GrabButtonEnabler;
        GameManager.Instance.onActivateQuestChange -= ShowQuestStartIndicator;

        startQuestIndicator.SetActive(false);

        inventoryStageOne = null;
        genderSetter = null;
    }

    IEnumerator TimerEnabler()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium ||
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            timerObj.SetActive(true);
        else
            timerObj.SetActive(false);
    }

    private void ShowQuestStartIndicator(object sender, EventArgs e)
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE)
        {
            if (GameManager.Instance.GetSetActivateQuestState)
                startQuestIndicator.SetActive(true);
            else
            {
                if (GameManager.Instance.questCount > 0)
                    finishQuestIndicator.SetActive(true);
                else if (GameManager.Instance.questCount <= 0)
                    bodyPartsAllFoundIndicator.SetActive(true);
            }
        }
    }

    public void OpenInventory()
    {
        GameManager.Instance.soundSystem.PlayButton(openPauseMenuClip);
        GameManager.Instance.inventoryStageOne.GetSetInventoryState
        = InventoryStageOne.InventoryState.INVENTORY;
    }

    public void OpenPauseMenu()
    {
        GameManager.Instance.soundSystem.PlayButton(openPauseMenuClip);
        GameManager.Instance.GetSetPauseState = GameManager.PauseState.PAUSE;
    }

    #region STAGE ONE

    private void GrabButtonEnabler(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetGameState != GameManager.GAMESTATE.GAMEPLAY)
            return;

        GrabButtonBodyPartChecker();
    }

    private void GrabButtonBodyPartChecker()
    {
        if (inventoryStageOne.OnTriggerEnterCharactrOnBodyPart == null)
            grabButton.interactable = false;

        else
            grabButton.interactable = true;
    }

    public void GrabButton()
    {
        StartCoroutine(GrabButtonStageOne());
    }

    IEnumerator GrabButtonStageOne()
    {
        if (canPickup && GameManager.Instance.inventoryStageOne.OnTriggerEnterCharactrOnBodyPart != null)
        {
            GameManager.Instance.questCount--;

            grabButton.interactable = false;
            GameManager.Instance.GetSetPickedupState = true;
            canPickup = false;

            if (GameManager.Instance.direction.magnitude == 0f)
            {
                GameManager.Instance.idlePickup = true;

                genderSetter.GetSetGenderAnimator.SetBool("isRunning", false);
                genderSetter.GetSetGenderAnimator.SetBool("isWalking", false);
                genderSetter.GetSetGenderAnimator.SetTrigger("idlePickup");
            }

            else if (GameManager.Instance.direction.magnitude > 0f && GameManager.Instance.direction.magnitude <= 0.5f)
            {
                GameManager.Instance.idlePickup = true;

                genderSetter.GetSetGenderAnimator.SetBool("isRunning", false);
                genderSetter.GetSetGenderAnimator.SetBool("isWalking", false);
                genderSetter.GetSetGenderAnimator.SetTrigger("idlePickup");
            }

            else if (GameManager.Instance.direction.magnitude > 0.5f && GameManager.Instance.direction.magnitude <= 1.5f)
            {
                grabButton.interactable = false;
                GameManager.Instance.idlePickup = false;
                genderSetter.GetSetGenderAnimator.SetTrigger("runningPickup");
                inventoryStageOne.InsertBodyPartOnList(inventoryStageOne.OnTriggerEnterCharactrOnBodyPart);

                GameManager.Instance.soundSystem.PlaySFX(pickupItemClip);

                //  for destroying game object
                //  and for nullifying body part
                Destroy(inventoryStageOne.GetSetBodyPartObj);
                inventoryStageOne.GetSetBodyPartObj = null;
                inventoryStageOne.OnTriggerEnterCharactrOnBodyPart = null;
            }
        }

        yield return new WaitForSeconds(nextGrabButtonDelay);

        GameManager.Instance.idlePickup = false;
        canPickup = true;
        GameManager.Instance.GetSetPickedupState = false;
    }

    #endregion
}
