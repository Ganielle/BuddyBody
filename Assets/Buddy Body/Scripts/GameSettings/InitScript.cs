using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTMP;
    [SerializeField] private string loadScene;
    [SerializeField] private AudioClip popup;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip backbutton;
    [SerializeField] private bool isDebugging;

    private void Start()
    {
        StartCoroutine(InitSettings());
    }

    IEnumerator InitSettings()
    {

        statsTMP.text = "Checking device specs..";
        GameManager.Instance.soundSystem.PlaySFX(popup);

        yield return new WaitForSeconds(3f);

        //  For Settings
        PlayerPrefs.SetFloat("bgvolume", 1f);
        PlayerPrefs.SetFloat("sfxvolume", 1f);
        PlayerPrefs.SetInt("easyTutorial", 0);
        PlayerPrefs.SetInt("mediumTutorial", 0);
        PlayerPrefs.SetInt("hardTutorial", 0);
        GameManager.Instance.soundSystem.GetSetBGVolume = PlayerPrefs.GetFloat("bgvolume");
        GameManager.Instance.soundSystem.GetSetSFXVolume = PlayerPrefs.GetFloat("sfxvolume");

        statsTMP.text = "Applying settings..";
        GameManager.Instance.soundSystem.PlaySFX(click);

        yield return new WaitForSeconds(3f);

        statsTMP.text = "Checking database..";
        GameManager.Instance.soundSystem.PlaySFX(click);

        if (isDebugging)
        {
            //  For Stages
            PlayerPrefs.SetInt("StageOneUnlock", 1);
            PlayerPrefs.SetInt("StageTwoUnlock", 1);

            //  For Difficulties
            PlayerPrefs.SetInt("StageOneHard", 1);
            PlayerPrefs.SetInt("StageTwoHard", 1);
        }
        else
        {
            //  For Stages
            PlayerPrefs.SetInt("StageOneUnlock", 1);
            PlayerPrefs.SetInt("StageTwoUnlock", 0);

            //  For Difficulties
            PlayerPrefs.SetInt("StageOneHard", 0);
            PlayerPrefs.SetInt("StageTwoHard", 0);
        }

        //  For leaderboards
        //  StageOne
        List<GameManager.HighScoreEntry> highScoreEntry = new List<GameManager.HighScoreEntry>() { };
        var jsonScoreEntry = JsonUtility.ToJson(highScoreEntry);

        PlayerPrefs.SetString("StageOneScoreEasy", jsonScoreEntry);
        PlayerPrefs.SetString("StageOneScoreMedium", jsonScoreEntry);
        PlayerPrefs.SetString("StageOneScoreHard", jsonScoreEntry);

        //  StageTwo
        PlayerPrefs.SetString("StageTwoScoreEasy", jsonScoreEntry);
        PlayerPrefs.SetString("StageTwoScoreMedium", jsonScoreEntry);
        PlayerPrefs.SetString("StageTwoScoreHard", jsonScoreEntry);

        yield return new WaitForSeconds(5f);

        statsTMP.text = "Initialization Complete..";
        GameManager.Instance.soundSystem.PlaySFX(click);

        yield return new WaitForSeconds(3f);

        GameManager.Instance.soundSystem.PlaySFX(backbutton);
        GameManager.Instance.sceneController.GetSetCurrentSceneName = loadScene;
        PlayerPrefs.SetInt("SetFirstInitializeData", 1);
    }
}
