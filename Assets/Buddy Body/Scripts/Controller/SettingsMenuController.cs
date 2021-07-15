using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject areYouSureObj;
    [SerializeField] private GameObject tutorialSelect;

    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI musicVolumeVal;
    [SerializeField] private TextMeshProUGUI sfxVolumeVal;

    [Header("Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Button")]
    [SerializeField] private Button saveButton;

    [Header("Audio")]
    [SerializeField] private AudioClip clickClip;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] int tempSFXVolume;
    [ReadOnly] [SerializeField] int tempBGVolume;
    [ReadOnly] [SerializeField] int sceneIndex;
    [ReadOnly] [SerializeField] bool canBack;

    private void OnEnable()
    {
        SetMusicVolumeAtStart();

        canBack = true;

        GameManager.Instance.soundSystem.onBGVolumeChange += CanSaveNewValue;
    }

    private void OnDisable()
    {
        GameManager.Instance.soundSystem.onBGVolumeChange -= CanSaveNewValue;
    }

    private void Update()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetKey(KeyCode.Escape) && canBack)
        {
            StartCoroutine(SystemBackButton());
        }
    }

    #region BACK

    IEnumerator SystemBackButton()
    {
        if (sceneIndex == 0)
            yield break;

        canBack = false;

        BackSettings();

        yield return new WaitForSecondsRealtime(0.5f);

        canBack = true;
    }

    private void BackSettings()
    {
        if (GameManager.Instance.GetSetPauseState != GameManager.PauseState.NONE)
            BackButton();
    }

    #endregion

    #region SETTINGS

    private void CanSaveNewValue(object sender, EventArgs e)
    {
        SetMusicVolumeAtStart();
        CheckIfVolumeChanges();
    }

    private void SetMusicVolumeAtStart()
    {
        tempSFXVolume = Convert.ToInt32(GameManager.Instance.soundSystem.GetSetSFXVolume * 100);
        tempBGVolume = Convert.ToInt32(GameManager.Instance.soundSystem.GetSetBGVolume * 100);

        musicVolumeVal.text = Convert.ToString(tempBGVolume);
        musicSlider.value = tempBGVolume;

        sfxVolumeVal.text = Convert.ToString(tempSFXVolume);
        sfxSlider.value = tempSFXVolume;
    }

    public void CheckIfVolumeChanges()
    {
        musicVolumeVal.text = Convert.ToString(musicSlider.value);
        sfxVolumeVal.text = Convert.ToString(sfxSlider.value);

        if (musicSlider.value != tempBGVolume || sfxSlider.value != tempSFXVolume)
            saveButton.interactable = true;
        else if (musicSlider.value == tempBGVolume && sfxSlider.value == tempSFXVolume)
            saveButton.interactable = false;
    }

    public void BackButton()
    {
        if (musicSlider.value == tempBGVolume &&
            sfxSlider.value == tempSFXVolume)
            Back();

        else if (musicSlider.value != tempBGVolume ||
            sfxSlider.value != tempSFXVolume)
        {

            if (areYouSureObj.activeSelf)
            {
                YesButton();
            }
            else
            {
                areYouSureObj.SetActive(true);
            }
        }
    }

    public void SaveChanges()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.soundSystem.GetSetSFXVolume = (float)sfxSlider.value / 100;
        GameManager.Instance.soundSystem.GetSetBGVolume = (float)musicSlider.value / 100;
    }

    public void YesButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        SetMusicVolumeAtStart();
        Back();
    }

    public void NoButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        SaveChanges();
        Back();
    }

    private void Back()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);

        if (areYouSureObj.activeSelf)
        {
            areYouSureObj.SetActive(false);
        }
        else
        {
            if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
            {
                if (tutorialSelect.activeSelf)
                    BackToSetting();
                else
                {
                    if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.SETTINGS)
                        GameManager.Instance.GetSetPauseState = GameManager.PauseState.PAUSE;

                    //if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.PAUSE)
                    //    GameManager.Instance.GetSetPauseState = GameManager.PauseState.NONE;
                }
            }
            else if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.MAINMENU)
            {
                if (tutorialSelect.activeSelf)
                    BackToSetting();
                else
                {
                    GameManager.Instance.GetSetPauseState = GameManager.PauseState.NONE;
                }
            }
        }
    }

    public void DefaultButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.soundSystem.GetSetSFXVolume = 1f;
        GameManager.Instance.soundSystem.GetSetBGVolume = 1f;
    }

    public void HowToPlay()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);

        tutorialSelect.SetActive(true);
    }

    #endregion

    #region TUTORIAL

    public void ControlsButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.CONTROLSTUTS;
    }

    public void MapsButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.MAPSTUTS;
    }

    public void InventoryButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.INVENTORYTUTS;
    }

    public void QuestButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.QUESTTUTS;
    }

    public void TimerButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.TIMERTUTS;
    }

    public void BackToSetting()
    {
        GameManager.Instance.soundSystem.PlayButton(clickClip);
        tutorialSelect.SetActive(false);
    }


    #endregion
}
