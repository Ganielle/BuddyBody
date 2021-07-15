using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject nextButtonObj;
    [SerializeField] private GameObject previousButtonObj;
    [SerializeField] private GameObject closeButtonObj;
    [SerializeField] private GameObject parentObj;
    [SerializeField] private List<GameObject> seenTutorials;
    [SerializeField] private List<GameObject> notSeenTutorials;
    [SerializeField] private List<GameObject> seeAllTuts;

    [Space]
    [SerializeField] private CanvasGroup tutorialCG;
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip closeClip;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] int currentIndex;
    [ReadOnly] [SerializeField] bool canTouch;
    [ReadOnly] [SerializeField] List<GameObject> tutorials;

    private event EventHandler changeCurrentIndex;
    public event EventHandler onChangeCurrentIndex
    {
        add
        {
            if (changeCurrentIndex == null || !changeCurrentIndex.GetInvocationList().Contains(value))
                changeCurrentIndex += value;
        }
        remove
        {
            changeCurrentIndex -= value;
        }
    }
    private int GetSetCurrentIndex
    {
        get { return currentIndex; }
        set
        {
            currentIndex = value;
            changeCurrentIndex?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnEnable()
    {
        tutorialCG.alpha = 0f;

        LeanTween.alphaCanvas(tutorialCG, 1f, 0.25f).setEase(LeanTweenType.easeInSine);

        canTouch = true;
        GetSetCurrentIndex = 0;

        onChangeCurrentIndex += ButtonEnabler;

        if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.SETTINGS)
        {
            tutorials = seeAllTuts;
        }
        else if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.NONE)
        {
            if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.EASYTUTS &&
                PlayerPrefs.GetInt("mediumTutorial") == 1)
                tutorials = seenTutorials;
            else if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.EASYTUTS &&
                PlayerPrefs.GetInt("mediumTutorial") == 0)
                tutorials = notSeenTutorials;

            if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.MEDIUMTUTS &&
                PlayerPrefs.GetInt("easyTutorial") == 1)
                tutorials = seenTutorials;
            else if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.MEDIUMTUTS &&
                PlayerPrefs.GetInt("easyTutorial") == 0)
                tutorials = notSeenTutorials;

            if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.HARDTUTS)
                tutorials = seenTutorials;
        }

        ButtonChecker();
        TutorialEnabler();
    }

    private void OnDisable()
    {
        onChangeCurrentIndex -= ButtonEnabler;
    }

    private void ButtonEnabler(object sender, EventArgs e)
    {
        ButtonChecker();
        TutorialEnabler();
    }

    private void ButtonChecker()
    {
        if (GetSetCurrentIndex > 0 && GetSetCurrentIndex < tutorials.Count - 1)
        {
            closeButtonObj.SetActive(false);
            nextButtonObj.SetActive(true);

            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        if (GetSetCurrentIndex == tutorials.Count - 1)
        {
            nextButtonObj.SetActive(false);
            closeButtonObj.SetActive(true);

            nextButton.interactable = false;
            previousButton.interactable = true;
        }

        if (GetSetCurrentIndex == 0)
        {
            closeButtonObj.SetActive(false);
            nextButtonObj.SetActive(true);

            nextButton.interactable = true;
            previousButton.interactable = false;
        }
    }

    private void TutorialEnabler()
    {
        if (GetSetCurrentIndex == 0)
        {
            tutorials[GetSetCurrentIndex + 1].SetActive(false);
            tutorials[0].SetActive(true);

            return;
        }
        else if (GetSetCurrentIndex == tutorials.Count - 1)
        {
            tutorials[GetSetCurrentIndex].SetActive(true);
            tutorials[GetSetCurrentIndex - 1].SetActive(false);

            return;
        }

        tutorials[GetSetCurrentIndex - 1].SetActive(false);
        tutorials[GetSetCurrentIndex].SetActive(true);
        tutorials[GetSetCurrentIndex + 1].SetActive(false);
    }

    public void NextButton()
    {
        GameManager.Instance.soundSystem.PlayButton(buttonClip); 
        GetSetCurrentIndex++;
    }

    public void PreviousButton()
    {
        GameManager.Instance.soundSystem.PlayButton(buttonClip);
        GetSetCurrentIndex--;
    }

    public void CloseButton()
    {
        GameManager.Instance.soundSystem.PlayButton(closeClip);
        LeanTween.alphaCanvas(tutorialCG, 0f, 0.25f).setEase(LeanTweenType.easeInSine).setOnComplete(() => {

            if (Time.timeScale == 0f)
                Time.timeScale = 1f;

            if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.NONE)
            {
                if (SceneManager.GetActiveScene().name == "StageOne" || SceneManager.GetActiveScene().name == "StageTwo")
                {
                    if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.EASYTUTS &&
                        PlayerPrefs.GetInt("easyTutorial") == 0)
                        PlayerPrefs.SetInt("easyTutorial", 1);

                    if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.MEDIUMTUTS &&
                        PlayerPrefs.GetInt("mediumTutorial") == 0)
                        PlayerPrefs.SetInt("mediumTutorial", 1);

                    if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.HARDTUTS &&
                        PlayerPrefs.GetInt("hardTutorial") == 0)
                        PlayerPrefs.SetInt("hardTutorial", 1);

                    GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.STARTCOUNTDOWN;
                }
            }

            GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.NONE;

            if (GetSetCurrentIndex == tutorials.Count - 1)
                tutorials[tutorials.Count - 1].SetActive(false);
            else
                tutorials[GetSetCurrentIndex].SetActive(false);

            gameObject.SetActive(false);
            parentObj.SetActive(false);
        });
    }
}
