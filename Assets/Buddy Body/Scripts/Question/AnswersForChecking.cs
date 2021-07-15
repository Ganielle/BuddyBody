using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswersForChecking : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image checkAnswer;
    [SerializeField] private Sprite rightAnswer;
    [SerializeField] private Sprite wrongAnswer;

    [Header("Scripts")]
    [SerializeField] private ImageColorTweenAnimation tween;
    [SerializeField] private QuestionItemStageOne questionItem;

    private void OnEnable()
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CHECKING)
        {
            checkAnswer.gameObject.SetActive(true);
            CheckAnswer();
        }
        else
            checkAnswer.gameObject.SetActive(false);
    }

    public void CheckAnswer()
    {
        if (questionItem.bodyPartObj == null)
            checkAnswer.sprite = wrongAnswer;
        else
        {
            if (questionItem.question.answer == questionItem.bodyPart.bodyPart.answer)
            {
                GameManager.Instance.totalRightAnswers += 1;
                GameManager.Instance.totalScore += 50;
                checkAnswer.sprite = rightAnswer;
            }
            else
                checkAnswer.sprite = wrongAnswer;
        }

        tween.PlayAnimation();
    }
}
