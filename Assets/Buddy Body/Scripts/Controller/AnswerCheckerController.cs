using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnswerCheckerController : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI gradeTMP;

    [Header("Gameobjects")]
    [SerializeField] private GameObject retryButtonObj;
    [SerializeField] private GameObject nextStageButtonObj;
    [SerializeField] private GameObject answerPanelObj;
    [SerializeField] private GameObject resultPanelObj;

    [Header("Music")]
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip defeat;

    [Header("Image")]
    [SerializeField] private Image fadeImage;

    private void OnEnable()
    {
        answerPanelObj.SetActive(false);
        resultPanelObj.SetActive(false);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        fadeImage.gameObject.SetActive(true);

        LeanTween.value(fadeImage.gameObject, a => fadeImage.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), 2f).
            setEase(LeanTweenType.easeInSine);


        yield return new WaitWhile(() => !GameManager.Instance.donePopulatingAnswers);

        yield return new WaitForSeconds(2f);

        answerPanelObj.SetActive(true);
        resultPanelObj.SetActive(true);

        LeanTween.value(fadeImage.gameObject, a => fadeImage.color = a, new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), 2f).
            setEase(LeanTweenType.easeInSine).setOnComplete(() => fadeImage.gameObject.SetActive(false));

        CheckRightAnswers();
    }

    private void CheckRightAnswers()
    {
        string stageLeaderboard = SceneManager.GetActiveScene().name + "Score" + GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty;

        if (GameManager.Instance.totalRightAnswers >= 8)
        {

            GameManager.Instance.soundSystem.ChangeBGMusic(victory, 41.5f, 5);

            if (SceneManager.GetActiveScene().name == "StageOne")
                nextStageButtonObj.SetActive(true);
            else
                nextStageButtonObj.SetActive(false);

            retryButtonObj.SetActive(false);

            gradeTMP.text = "PASSED";

            if (PlayerPrefs.GetInt("StageTwoUnlock") == 0)
                PlayerPrefs.SetInt("StageTwoUnlock", 1);

            if (SceneManager.GetActiveScene().name == "StageOne")
            {
                if (PlayerPrefs.GetInt("StageOneHard") == 0)
                    PlayerPrefs.SetInt("StageOneHard", 1);
            }
            else if (SceneManager.GetActiveScene().name == "StageTwo")
            {
                if (PlayerPrefs.GetInt("StageTwoHard") == 0)
                    PlayerPrefs.SetInt("StageTwoHard", 1);
            }
        }
        else
        {
            GameManager.Instance.soundSystem.ChangeBGMusic(defeat, 105f, 0f);

            retryButtonObj.SetActive(true);
            nextStageButtonObj.SetActive(false);

            gradeTMP.text = "FAILED";
        }

        GameManager.Instance.totalScore += Convert.ToInt32(GameManager.Instance.GetSetTimer);

        GameManager.Instance.SaveHighScore(GameManager.Instance.totalScore, 
            GameManager.Instance.GetSetPlayerName, stageLeaderboard, gradeTMP.text);
        scoreTMP.text = GameManager.Instance.totalScore.ToString();
        GameManager.Instance.donePopulatingAnswers = false;
    }
}
