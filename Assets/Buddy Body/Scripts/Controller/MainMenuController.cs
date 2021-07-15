using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clickButtonClip;
    [SerializeField] private AudioClip backButtonClip;
    [SerializeField] private AudioClip playGameButtonClip;
    [SerializeField] private AudioClip selectItemClip;

    [Header("MainMenu")]
    [SerializeField] private CanvasGroup mainMenuCG;

    [Header("Leaderboards")]
    [SerializeField] private CanvasGroup leaderboardsCG;

    [Header("Stage Select")]
    [SerializeField] private Color selectedStageColor;
    [SerializeField] private Button stageOneButton;
    [SerializeField] private Button stageTwoButton;
    [SerializeField] private Button stageNextButton;
    [SerializeField] private Image stageOneImage;
    [SerializeField] private Image stageTwoImage;
    [SerializeField] private Image stageOneButtonImage;
    [SerializeField] private Image stageTwoButtonImage;
    [SerializeField] private GameObject stageOneLockIcon;
    [SerializeField] private GameObject stageTwoLockIcon;
    [SerializeField] private CanvasGroup stageSelectCG;

    [Header("Difficulties")]
    [SerializeField] private Color selectedDifficultyColor;
    [SerializeField] private Color unSelectedDifficultyColor;
    [SerializeField] private Button hardDifficultyButton;
    [SerializeField] private Button nextDifficultyButton;
    [SerializeField] private CanvasGroup difficultyCG;
    [SerializeField] private Image easyButtonImage;
    [SerializeField] private Image mediumButtonImage;
    [SerializeField] private Image hardButtonImage;

    [Header("Select Gender")]
    [SerializeField] private Animator boyAnimator;
    [SerializeField] private Animator girlAnimator;
    [SerializeField] private Image maleButtonImage;
    [SerializeField] private Image girlButtonImage;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button playButton;
    [SerializeField] private GraphicRaycaster playGR;
    [SerializeField] private CanvasGroup selectGenderCG;

    [Header("Quit Game")]
    [SerializeField] private CanvasGroup areYouSureCG;
    [SerializeField] private Button yesButtonQuit;
    [SerializeField] private Button noButtonQuit;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] Question.Difficulty stageDifficulty;
    [ReadOnly] [SerializeField] MainMenu.MainMenuState mainMenuState;
    [ReadOnly] [SerializeField] MainMenu.MainMenuState lastMainMenuState;
    [ReadOnly] [SerializeField] int sceneIndex;
    [ReadOnly] [SerializeField] bool canBack;

    private void Awake()
    {
        GameManager.Instance.playerObj = null;

        canBack = true;

        mainMenuState = MainMenu.MainMenuState.MAINMENU;
        GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
    }

    private void OnEnable()
    {
        GameManager.Instance.mainMenu.onChangeMainMenuState += RefreshContent;
        GameManager.Instance.mainMenu.onChangeStageSelected += StageSelect;
        GameManager.Instance.levelGeneratorDifficulty.onChangeLevelDifficulty += Difficulty;
        GameManager.Instance.genderSetter.onChangeGender += ChangeGender;
    }

    private void OnDisable()
    {
        GameManager.Instance.mainMenu.onChangeMainMenuState -= RefreshContent;
        GameManager.Instance.mainMenu.onChangeStageSelected -= StageSelect;
        GameManager.Instance.levelGeneratorDifficulty.onChangeLevelDifficulty -= Difficulty;
        GameManager.Instance.genderSetter.onChangeGender -= ChangeGender;
    }

    private void Start()
    {
        DifficultySelectedButtonChange();
        CheckNameAndCharacter();
    }

    private void Update()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetKey(KeyCode.Escape) && canBack)
        {
            StartCoroutine(SystemBackButton());
        }
    }

    private void RefreshContent(object sender, EventArgs e)
    {
        StartCoroutine(DisablePreviousMainMenuState());
        ContentRefresher();
    }

    private void ContentRefresher()
    {
        if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.STAGESELECT)
            RefreshStageSelectContent();
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.DIFFICULTYSELECT)
            RefreshDifficultyContent();
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.GENDERSELECT)
            SelectCharacterRefresherContent();
    }

    IEnumerator DisablePreviousMainMenuState()
    {
        if (lastMainMenuState == MainMenu.MainMenuState.MAINMENU)
        {
            mainMenuCG.alpha = 1f;
            LeanTween.alphaCanvas(mainMenuCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                mainMenuCG.gameObject.SetActive(false);
            });
        }
        else if (lastMainMenuState == MainMenu.MainMenuState.LEADERBOARDS)
        {
            leaderboardsCG.alpha = 1f;
            LeanTween.alphaCanvas(leaderboardsCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                leaderboardsCG.gameObject.SetActive(false);
            });
        }
        else if (lastMainMenuState == MainMenu.MainMenuState.STAGESELECT)
        {
            stageSelectCG.alpha = 1f;
            LeanTween.alphaCanvas(stageSelectCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                stageSelectCG.gameObject.SetActive(false);
            });
        }
        else if (lastMainMenuState == MainMenu.MainMenuState.DIFFICULTYSELECT)
        {
            difficultyCG.alpha = 1f;
            LeanTween.alphaCanvas(difficultyCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                difficultyCG.gameObject.SetActive(false);
            });
        }
        else if (lastMainMenuState == MainMenu.MainMenuState.GENDERSELECT)
        {
            selectGenderCG.alpha = 1f;
            LeanTween.alphaCanvas(selectGenderCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                selectGenderCG.gameObject.SetActive(false);
            });
        }
        else if (lastMainMenuState == MainMenu.MainMenuState.QUITGAME)
        {
            areYouSureCG.alpha = 1f;
            LeanTween.alphaCanvas(areYouSureCG, 0f, 0.25f).setEase(GameManager.Instance.easeType).setOnComplete(() => {
                areYouSureCG.gameObject.SetActive(false);
            });
        }

        yield return new WaitForSecondsRealtime(0.25f);

        if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.MAINMENU)
        {
            mainMenuCG.alpha = 0f;
            mainMenuCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(mainMenuCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.STAGESELECT)
        {
            stageSelectCG.alpha = 0f;
            stageSelectCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(stageSelectCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.DIFFICULTYSELECT)
        {
            difficultyCG.alpha = 0f;
            difficultyCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(difficultyCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.GENDERSELECT)
        {
            selectGenderCG.alpha = 0f;
            selectGenderCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(selectGenderCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.LEADERBOARDS)
        {
            leaderboardsCG.alpha = 0f;
            leaderboardsCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(leaderboardsCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.QUITGAME)
        {
            areYouSureCG.alpha = 0f;
            areYouSureCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(areYouSureCG, 1f, 0.25f).setEase(GameManager.Instance.easeType);
        }
    }

    #region MAIN MENU

    public void StartGameButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        lastMainMenuState = GameManager.Instance.mainMenu.GetSetMainMenuState;
        mainMenuState = MainMenu.MainMenuState.STAGESELECT;
        GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
    }

    public void Leaderboard()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        lastMainMenuState = GameManager.Instance.mainMenu.GetSetMainMenuState;
        mainMenuState = MainMenu.MainMenuState.LEADERBOARDS;
        GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
    }

    public void Settings()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        GameManager.Instance.GetSetPauseState = GameManager.PauseState.SETTINGS;
    }

    public void QuitGame()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        lastMainMenuState = GameManager.Instance.mainMenu.GetSetMainMenuState;
        mainMenuState = MainMenu.MainMenuState.QUITGAME;
        GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
    }

    public void AreYouSureQuit()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);

        yesButtonQuit.interactable = false;
        noButtonQuit.interactable = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region STAGE SELECT

    private void StageSelect(object sender, EventArgs e)
    {
        if (!GameManager.Instance.mainMenu.GetSetSelectedScene.Equals(""))
            stageNextButton.interactable = true;
        else
            stageNextButton.interactable = false;

        if (GameManager.Instance.mainMenu.GetSetSelectedScene.Equals("StageOne"))
        {
            stageOneButtonImage.color = selectedStageColor;
            stageTwoButtonImage.color = stageTwoButton.colors.normalColor;
        }
        else if (GameManager.Instance.mainMenu.GetSetSelectedScene.Equals("StageTwo"))
        {
            stageOneButtonImage.color = stageOneButton.colors.normalColor;
            stageTwoButtonImage.color = selectedStageColor;
        }
        else
        {
            stageOneButtonImage.color = stageOneButton.colors.normalColor;
            stageTwoButtonImage.color = stageTwoButton.colors.normalColor;
        }
    }

    private void RefreshStageSelectContent()
    {
        if (PlayerPrefs.GetInt("StageOneUnlock") == 1)
        {
            stageOneLockIcon.SetActive(false);
            stageOneButton.interactable = true;
            stageOneImage.color = stageOneButton.colors.normalColor;
        }
        else
        {
            stageOneLockIcon.SetActive(true);
            stageOneButton.interactable = false;
            stageOneImage.color = stageOneButton.colors.disabledColor;
        }

        if (PlayerPrefs.GetInt("StageTwoUnlock") == 1)
        {
            stageTwoLockIcon.SetActive(false);
            stageTwoButton.interactable = true;
            stageTwoImage.color = stageTwoButton.colors.normalColor;
        }
        else
        {
            stageTwoLockIcon.SetActive(true);
            stageTwoButton.interactable = false;
            stageTwoImage.color = stageTwoButton.colors.disabledColor;
        }
    }

    public void StageSelectButton(string value)
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        GameManager.Instance.mainMenu.GetSetSelectedScene = value;
    }

    public void ToDifficulty()
    {
        if (!GameManager.Instance.mainMenu.GetSetSelectedScene.Equals(""))
        {
            GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
            lastMainMenuState = mainMenuState;
            mainMenuState = MainMenu.MainMenuState.DIFFICULTYSELECT;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
    }

    #endregion

    #region DIFFICULTY

    private void Difficulty(object sender, EventArgs e)
    {
        DifficultySelectedButtonChange();
        DifficultyNextButton();
    }

    private void RefreshDifficultyContent()
    {
        if (GameManager.Instance.mainMenu.GetSetSelectedScene.Equals("StageOne"))
        {
            if (PlayerPrefs.GetInt("StageOneHard") == 0)
                hardDifficultyButton.interactable = false;
            else
                hardDifficultyButton.interactable = true;
        }
        else if (GameManager.Instance.mainMenu.GetSetSelectedScene.Equals("StageTwo"))
        {

            if (PlayerPrefs.GetInt("StageTwoHard") == 0)
                hardDifficultyButton.interactable = false;
            else
                hardDifficultyButton.interactable = true;
        }
    }

    private void DifficultySelectedButtonChange()
    {
        easyButtonImage.color = unSelectedDifficultyColor;
        mediumButtonImage.color = unSelectedDifficultyColor;
        hardButtonImage.color = unSelectedDifficultyColor;

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy)
            easyButtonImage.color = selectedDifficultyColor;
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium)
            mediumButtonImage.color = selectedDifficultyColor;
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            hardButtonImage.color = selectedDifficultyColor;
    }

    private void DifficultyNextButton()
    {
        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.NONE)
            nextDifficultyButton.interactable = false;
        else
            nextDifficultyButton.interactable = true;
    }

    public void EasyButton()
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        stageDifficulty = Question.Difficulty.Easy;
        GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty = stageDifficulty;
    }

    public void MediumButton()
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        stageDifficulty = Question.Difficulty.Medium;
        GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty = stageDifficulty;
    }

    public void HardButton()
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        stageDifficulty = Question.Difficulty.Hard;
        GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty = stageDifficulty;
    }

    public void ToSelectGender()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        lastMainMenuState = MainMenu.MainMenuState.DIFFICULTYSELECT;
        mainMenuState = MainMenu.MainMenuState.GENDERSELECT;
        GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
    }

    #endregion

    #region SELECT CHARACTER

    private void ChangeGender(object sender, EventArgs e)
    {
        SelectCharacterRefresherContent();
    }

    private void SelectCharacterRefresherContent()
    {
        CheckNameAndCharacter();
        maleButtonImage.color = new Color(255, 255, 255);
        girlButtonImage.color = new Color(255, 255, 255);
        nameInputField.interactable = false;

        if (GameManager.Instance.genderSetter.GetSetGender == GenderSetter.Gender.MALE)
        {
            boyAnimator.SetTrigger("CharacterSelected");
            maleButtonImage.color = selectedStageColor;
            nameInputField.interactable = true;
        }
        else if (GameManager.Instance.genderSetter.GetSetGender == GenderSetter.Gender.FEMALE)
        {
            girlAnimator.SetTrigger("CharacterSelected");
            girlButtonImage.color = selectedStageColor;
            nameInputField.interactable = true;
        }
    }

    public void MaleButton()
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        GameManager.Instance.genderSetter.GetSetGender = GenderSetter.Gender.MALE;
    }

    public void FemaleButton()
    {
        GameManager.Instance.soundSystem.PlayButton(selectItemClip);
        GameManager.Instance.genderSetter.GetSetGender = GenderSetter.Gender.FEMALE;
    }

    public void CheckNameAndCharacter()
    {
        if (GameManager.Instance.genderSetter.GetSetGender != GenderSetter.Gender.NONE)
        {
            if (nameInputField.text.Length > 1)
            {
                if (nameInputField.characterValidation == InputField.CharacterValidation.Name)
                    playButton.interactable = true;
                else
                    playButton.interactable = false;
            }
            else
                playButton.interactable = false;

            string text = nameInputField.text;//get text from input field
            text = text.Replace(" ", "");//fliter spaces from text
            nameInputField.text = text;//set the text in the input field to the filtered text
        }
        else if (GameManager.Instance.genderSetter.GetSetGender == GenderSetter.Gender.NONE)
        {
            playButton.interactable = false;
            nameInputField.text = "";
        }
    }

    public void PlayGame()
    {
        GameManager.Instance.soundSystem.PlayButton(playGameButtonClip);
        playGR.enabled = false;
        int rand = UnityEngine.Random.Range(1,3);

        if (rand == 1 || rand == 3)
            GameManager.Instance.levelGeneratorDifficulty.GetSetSubTopic = Question.SubTopic.UPPER;
        else if (rand == 2)
            GameManager.Instance.levelGeneratorDifficulty.GetSetSubTopic = Question.SubTopic.LOWER;

        GameManager.Instance.GetSetPlayerName = nameInputField.text;
        GameManager.Instance.sceneController.GetSetCurrentSceneName = GameManager.Instance.mainMenu.GetSetSelectedScene;
    }

    #endregion

    public void BackButton()
    {
        GameManager.Instance.soundSystem.PlayButton(backButtonClip);

        if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.MAINMENU)
        {
            lastMainMenuState = MainMenu.MainMenuState.MAINMENU;
            mainMenuState = MainMenu.MainMenuState.QUITGAME;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.GENDERSELECT)
        {
            GameManager.Instance.genderSetter.GetSetGender = GenderSetter.Gender.NONE;
            lastMainMenuState = MainMenu.MainMenuState.GENDERSELECT;
            mainMenuState = MainMenu.MainMenuState.DIFFICULTYSELECT;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.DIFFICULTYSELECT)
        {
            stageDifficulty = Question.Difficulty.NONE;
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty = stageDifficulty;
            lastMainMenuState = MainMenu.MainMenuState.DIFFICULTYSELECT;
            mainMenuState = MainMenu.MainMenuState.STAGESELECT;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.STAGESELECT)
        {
            GameManager.Instance.mainMenu.GetSetSelectedScene = "";
            lastMainMenuState = MainMenu.MainMenuState.STAGESELECT;
            mainMenuState = MainMenu.MainMenuState.MAINMENU;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.LEADERBOARDS)
        {
            if (GameManager.Instance.GetSetShowGradeStats)
            {
                GameManager.Instance.GetSetShowGradeStats = false;
            }
            else
            {
                lastMainMenuState = MainMenu.MainMenuState.LEADERBOARDS;
                mainMenuState = MainMenu.MainMenuState.MAINMENU;
                GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
            }
        }
        else if (GameManager.Instance.mainMenu.GetSetMainMenuState == MainMenu.MainMenuState.QUITGAME)
        {
            lastMainMenuState = MainMenu.MainMenuState.QUITGAME;
            mainMenuState = MainMenu.MainMenuState.MAINMENU;
            GameManager.Instance.mainMenu.GetSetMainMenuState = mainMenuState;
        }
    }

    IEnumerator SystemBackButton()
    {
        if (sceneIndex == 0)
            yield break;

        canBack = false;

        Back();

        yield return new WaitForSecondsRealtime(0.5f);

        canBack = true;
    }

    private void Back()
    {
        if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.NONE)
            BackButton();
    }
}
