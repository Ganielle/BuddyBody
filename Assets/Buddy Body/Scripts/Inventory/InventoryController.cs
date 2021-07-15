using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI descriptionTxt;

    [Header("Button")]
    [SerializeField] private Button removeAnswerButton;

    [Header("Gameobject")]
    [SerializeField] private GameObject itemListContentGO;
    [SerializeField] private GameObject bodyPartItemInstantiate;
    [SerializeField] private GameObject inventoryButtons;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject areYouSure;

    private void OnEnable()
    {
        areYouSure.SetActive(false);
        inventoryButtons.SetActive(false);

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium ||
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            timer.SetActive(true);
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy)
            timer.SetActive(false);

        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart += Description;
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart += VideoBodyPart;
        GameManager.Instance.inventoryStageOne.onSelectAnsweredChange += RemoveAnswer;

        StartCoroutine(GameManager.Instance.StageOneItemList(GameManager.Instance.inventoryStageOne.GetBodyPartList(),
            bodyPartItemInstantiate, itemListContentGO.transform));

        DescriptionSetter();
        CheckIfRemoveAnswer();
    }

    private void OnDisable()
    {

        GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory = null;
        GameManager.Instance.inventoryStageOne.GetSetBodyPartObj = null;

        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart -= Description;
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart -= VideoBodyPart;
        GameManager.Instance.inventoryStageOne.onSelectAnsweredChange -= RemoveAnswer;
    }

    private void RemoveAnswer(object sender, EventArgs e)
    {
        CheckIfRemoveAnswer();
    }

    private void CheckIfRemoveAnswer()
    {
        if (GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj != null)
            removeAnswerButton.interactable = true;
        else
            removeAnswerButton.interactable = false;
    }

    public void RemoveAnswer()
    {
        if (!GameManager.Instance.GetSetRemoveItemState)
            GameManager.Instance.GetSetRemoveItemState = true;
    }

    #region VIDEO

    private void VideoBodyPart(object sender, EventArgs e)
    {
        if (GameManager.Instance.inventoryStageOne.GetSetInventoryState == InventoryStageOne.InventoryState.INVENTORY)
            StartCoroutine(CheckForVideo());
    }

    IEnumerator CheckForVideo()
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory != null)
            inventoryButtons.SetActive(true);
        else
            inventoryButtons.SetActive(false);
    }

    #endregion

    #region ITEM LIST

    private void Description(object sender, EventArgs e)
    {
        DescriptionSetter();
    }

    private void DescriptionSetter()
    {
        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory != null)
            descriptionTxt.text = GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory.description;
        else
            descriptionTxt.text = "";
    }

    #endregion

    public void CloseInventory()
    {
        GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj = null;
        GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPart = null;
        GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartItem = null;
        GameManager.Instance.inventoryStageOne.GetSetInventoryState = InventoryStageOne.InventoryState.NONE;
    }

    public void UnselectItem()
    {
        GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj = null;
        GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory = null;
        GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory = null;
    }

    public void PlayVideo()
    {
        GameManager.Instance.afterWatchVideo = true;
        GameManager.Instance.lastGameState = GameManager.GAMESTATE.GAMEPLAY;
        GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.WATCHVIDEO;
    }

    public void CheckAnswer()
    {
        GameManager.Instance.pauseTimer = true;
        areYouSure.SetActive(true);
    }

    public void NoButton()
    {
        GameManager.Instance.pauseTimer = false;
        areYouSure.SetActive(false);
    }

    public void YesButton()
    {
        Time.timeScale = 0f;
        GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.CHECKING;
    }
}
