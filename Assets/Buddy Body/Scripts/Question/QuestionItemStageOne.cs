using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestionItemStageOne : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Rect Transform")]
    [SerializeField] private RectTransform answerSlot;

    [Header("Graphic Raycast")]
    [SerializeField] private GraphicRaycaster answerGR;

    [Header("Button")]
    [SerializeField] private Button answerButton;

    [Header("Debugger")]
    [ReadOnly] public Question question;
    [ReadOnly] public BodyPartItemData bodyPart;
    [ReadOnly] public GameObject bodyPartObj;

    private void OnEnable()
    {
        questionText.text = question.answer;
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart += ButtonActivator;
        GameManager.Instance.onRemoveItemStateChange += RemoveItem;

        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CHECKING)
            answerGR.enabled = false;
        else
            answerGR.enabled = true;

        StartCoroutine(ButtonEnabler());
    }

    private void OnDisable()
    {
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart -= ButtonActivator;
        GameManager.Instance.onRemoveItemStateChange -= RemoveItem;
    }

    private void RemoveItem(object sender, EventArgs e)
    {
        StartCoroutine(RemoveAnswer());
    }

    IEnumerator RemoveAnswer()
    {
        if (GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj != null && bodyPartObj != null)
        {
            if (GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartItem.bodyPart.uniqueID == bodyPart.uniqueID)
            {
                GameManager.Instance.inventoryStageOne.GetBodyPartList().Find(body => body.uniqueID.Equals(bodyPart.uniqueID)).
                    GetSetAnswerItemState = false;

                Destroy(bodyPartObj);
                bodyPartObj = null;
                bodyPart = null;

                GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj = null;
                GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPart = null;
                GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartItem = null;
            }

            yield return new WaitForSecondsRealtime(0.5f);

            GameManager.Instance.GetSetRemoveItemState = false;
        }
    }

    private void ButtonActivator(object sender, EventArgs e)
    {
        StartCoroutine(ButtonEnabler());
    }

    IEnumerator ButtonEnabler()
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory != null)
        {
            if (GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory.bodyPart.GetSetAnswerItemState)
                answerButton.interactable = false;
            else
                answerButton.interactable = true;
        }
        else
            answerButton.interactable = true;
    }

    public void AnswerBox() => StartCoroutine(SelectAnswerBox());

    IEnumerator SelectAnswerBox()
    {
        //  SET ANSWER TO BOX
        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory != null &&
            GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj == null)
        {
            if (bodyPart != null && bodyPartObj != null)
            {
                GameManager.Instance.inventoryStageOne.GetBodyPartList().Find(body => body.uniqueID.Equals(bodyPart.uniqueID)).
                    GetSetAnswerItemState = false;

                Destroy(bodyPartObj);
                bodyPartObj = null;
                bodyPart = null;
            }

            //  Instantiating another selected body part answer in inventory to answer box
            foreach (BodyPartItemData item in GameManager.Instance.inventoryStageOne.GetBodyPartList())
            {
                if (item.uniqueID == GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory.bodyPart.uniqueID)
                {
                    item.GetSetAnswerItemState = true;
                    break;
                }

                yield return null;
            }

            GameObject gameobject = Instantiate(GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj);

            gameobject.transform.SetParent(answerSlot.transform, false);
            gameobject.GetComponent<RectTransform>().sizeDelta = answerSlot.sizeDelta;
            gameobject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            gameobject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            gameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            gameobject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            gameobject.GetComponent<Button>().enabled = false;
            gameobject.GetComponent<GraphicRaycaster>().enabled = false;

            bodyPart = gameobject.GetComponent<BodyPartItem>().bodyPart;
            bodyPartObj = gameobject;

            //  Clearing selected body part answer in inventory
            GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj = null;
            GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory = null;
            GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory = null;
        }

        //  SELECT ANSWER ON BOX IF EXISTS
        else if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory == null &&
           GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj == null)
        {
            GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartObj = bodyPartObj;
            GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPart = bodyPart.bodyPart;
            GameManager.Instance.inventoryStageOne.GetSetAnswerBodyPartItem = bodyPartObj.GetComponent<BodyPartItem>();
        }
    }
}
