using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color completeColor;
    [SerializeField] private Image indicator;
    [SerializeField] private TextMeshProUGUI questDetails;
    [SerializeField] private CanvasGroup questCG;
    [SerializeField] private Animator animator;

    [Header("Debugger")]
    [ReadOnly] public BodyPartItemData bodyPart;

    private void OnEnable()
    {
        GameManager.Instance.inventoryStageOne.onBodyPartAddOnList += CheckIfQuestIsFinished;
    }

    private void OnDisable()
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.MAINMENU ||
            GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.NONE)
        {
            DeleteQuestItem();

        }

        GameManager.Instance.inventoryStageOne.onBodyPartAddOnList -= CheckIfQuestIsFinished;
    }

    private void Start()
    {
        questDetails.text = "Find the " + bodyPart.bodyPart.answer + " body part";
    }

    private void CheckIfQuestIsFinished(object sender, EventArgs e)
    {
        DeleteQuestItem();
    }

    private void DeleteQuestItem()
    {
        if (GameManager.Instance.inventoryStageOne.GetBodyPartList().Exists(bodypart => bodypart.uniqueID == bodyPart.uniqueID))
        {
            animator.enabled = false;
            transform.SetAsFirstSibling();
            LeanTween.value(indicator.gameObject, c => indicator.color = c, new Color(indicator.color.r, 
                indicator.color.g, indicator.color.b), completeColor, 0.5f).setEase(LeanTweenType.easeInSine).
                setOnComplete(() => {
                    LeanTween.alphaCanvas(questCG, 0f, 0.5f).setEase(LeanTweenType.easeInSine).setDelay(2f).setOnComplete(() =>
                    {
                        Destroy(gameObject);
                    });
                });
        }
    }
}
