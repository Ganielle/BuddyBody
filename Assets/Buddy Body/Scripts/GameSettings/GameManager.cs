using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // ================================================================================

    public enum GAMESTATE
    {
        NONE,
        GAMEPLAY,
        CUTSCENE,
        MAINMENU,
        WATCHVIDEO,
        CHECKING,
        GAMEOVER
    }

    private event EventHandler gameStateChange;
    public event EventHandler onGameStateChange
    {
        add
        {
            if (gameStateChange == null || !gameStateChange.GetInvocationList().Contains(value))
                gameStateChange += value;
        }
        remove
        {
            gameStateChange -= value;
        }
    }
    public GAMESTATE GetSetGameState
    {
        get { return gameState; }
        set 
        { 
            gameState = value;
            gameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum Stage
    {
        NONE,
        LEVELONE,
        LEVELTWO,
        LEVELTHREE
    }
    private event EventHandler stageChange;
    public event EventHandler onStageChange
    {
        add
        {
            if (stageChange == null || !stageChange.GetInvocationList().Contains(value))
                stageChange += value;
        }
        remove
        {
            stageChange -= value;
        }
    }
    Stage stage;
    public Stage GetSetStage
    {
        get { return stage; }
        set
        {
            stage = value;
            stageChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum FindStartState
    {
        NONE,
        STARTCOUNTDOWN,
    }
    private event EventHandler findStartStateChange;
    public event EventHandler onFindStartStateChange
    {
        add
        {
            if (findStartStateChange == null || !findStartStateChange.GetInvocationList().Contains(value))
                findStartStateChange += value;
        }
        remove { findStartStateChange -= value; }
    }
    public FindStartState GetSetFindStartState
    {
        get { return findStartState; }
        set
        {
            findStartState = value;
            findStartStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum PauseState
    {
        NONE,
        PAUSE,
        SETTINGS
    }
    private event EventHandler pauseStateChange;
    public event EventHandler onPauseStateChange
    {
        add
        {
            if (pauseStateChange == null || !pauseStateChange.GetInvocationList().Contains(value))
                pauseStateChange += value;
        }
        remove { pauseStateChange -= value; }
    }
    public PauseState GetSetPauseState
    {
        get { return pauseState; }
        set
        {
            pauseState = value;
            pauseStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum TutorialState
    {
        NONE,
        EASYTUTS,
        MEDIUMTUTS,
        HARDTUTS,
        CONTROLSTUTS,
        MAPSTUTS,
        INVENTORYTUTS,
        QUESTTUTS,
        TIMERTUTS
    }
    private event EventHandler tutorialStateChange;
    public event EventHandler onTutorialStateChange
    {
        add
        {
            if (tutorialStateChange == null || !tutorialStateChange.GetInvocationList().Contains(value))
                tutorialStateChange += value;
        }
        remove { tutorialStateChange -= value; }
    }
    public TutorialState GetSetTutorialState
    {
        get { return tutorialState; }
        set
        {
            tutorialState = value;
            tutorialStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler nameChange;
    public event EventHandler onNameChange
    {
        add
        {
            if (nameChange == null || !nameChange.GetInvocationList().Contains(value))
                nameChange += value;
        }
        remove { nameChange -= value; }
    }
    public string GetSetPlayerName
    {
        get { return playerName; }
        set
        {
            playerName = value;
            nameChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler pickedupChange;
    public event EventHandler onpickedupChange
    {
        add
        {
            if (pickedupChange == null || !pickedupChange.GetInvocationList().Contains(value))
                pickedupChange += value;
        }
        remove
        {
            pickedupChange -= value;
        }
    }
    public bool GetSetPickedupState
    {
        get { return pickedup; }
        set
        {
            pickedup = value;
            pickedupChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler activateQuestChange;
    public event EventHandler onActivateQuestChange
    {
        add
        {
            if (activateQuestChange == null || !activateQuestChange.GetInvocationList().Contains(value))
                activateQuestChange += value;
        }
        remove
        {
            activateQuestChange -= value;
        }
    }
    public bool GetSetActivateQuestState
    {
        get { return activateQuest; }
        set
        {
            activateQuest = value;
            activateQuestChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler removeItemStateChange;
    public event EventHandler onRemoveItemStateChange
    {
        add
        {
            if (removeItemStateChange == null || !removeItemStateChange.GetInvocationList().Contains(value))
                removeItemStateChange += value;
        }
        remove
        {
            removeItemStateChange -= value;
        }
    }
    public bool GetSetRemoveItemState
    {
        get { return removeAnswer; }
        set
        {
            removeAnswer = value;
            removeItemStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler timerChange;
    public event EventHandler onTimerChange
    {
        add
        {
            if (timerChange == null || !timerChange.GetInvocationList().Contains(value))
                timerChange += value;
        }
        remove { timerChange -= value; }
    }
    public float GetSetTimer
    {
        get { return timer; }
        set
        {
            timer = value;
            timerChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler onMapStateChange;
    public event EventHandler mapStateChange
    {
        add
        {
            if (onMapStateChange == null || !onMapStateChange.GetInvocationList().Contains(value))
                onMapStateChange += value;
        }
        remove { onMapStateChange -= value; }
    }
    public bool GetSetMapState
    {
        get { return showMap; }
        set
        {
            showMap = value;
            onMapStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler passFailStatsChange;
    public event EventHandler onPassFailStatsChange
    {
        add
        {
            if (passFailStatsChange == null || !passFailStatsChange.GetInvocationList().Contains(value))
                passFailStatsChange += value;
        }
        remove { passFailStatsChange -= value; }
    }
    public bool GetSetShowGradeStats
    {
        get { return showGradeStats; }
        set
        {
            showGradeStats = value;
            passFailStatsChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public string GetSetGradeStats
    {
        get { return gradeStats; }
        set { gradeStats = value; }
    }

    // ================================================================================

    public SceneController sceneController = new SceneController();
    public GenderSetter genderSetter = new GenderSetter();
    public InventoryStageOne inventoryStageOne = new InventoryStageOne();
    public MainMenu mainMenu = new MainMenu();

    // ================================================================================

    [Header("Game Settings")]
    [SerializeField] private float gravityStrength;
    public Camera mainCamera;

    [Header("Scripts")]
    public VideoPlayerController videoPlayerController;
    public AnalogStick analogStick;
    [SerializeField] private ImageColorTweenAnimation loadingFade;
    [SerializeField] private CanvasGroupAlphaAnimation gameControllerCanvas;
    public QuestionGenerator questionGenerator;
    public LevelGeneratorDifficulty levelGeneratorDifficulty;
    public SoundSystem soundSystem;
    public TimerController timerController;

    [Header("GameObjects")]
    public GameObject loadingCanvas;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject gameplayControllers;
    [SerializeField] private List<GameObject> gameplayObjs;
    public GameObject questListObj;
    public GameObject answerListObj;
    public GameObject overviewMap;

    [Header("Loading Screen")]
    public bool startAtFullAlpha;
    [SerializeField] private string startupScene;
    [SerializeField] private float loadSpeedAnimation;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup gameControllerCG;

    [Header("Image")]
    [SerializeField] private Image loadingFadeImage;

    [Header("LeanTween")]
    public LeanTweenType easeType;

    [Header("Sounds")]
    [SerializeField] private AudioClip click;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] GAMESTATE gameState;
    [ReadOnly] public GAMESTATE lastGameState;
    [ReadOnly] [SerializeField] FindStartState findStartState;
    [ReadOnly] [SerializeField] PauseState pauseState;
    [ReadOnly] [SerializeField] TutorialState tutorialState;
    [ReadOnly] public GameObject playerObj;
    [ReadOnly] [SerializeField] string playerName;
    [ReadOnly] [SerializeField] float timer;
    [ReadOnly] public float totalSceneProgress;
    [ReadOnly] public float totalQuestionGeneratedProgress;
    [ReadOnly] public bool doneLoadingScene;
    [ReadOnly] public bool stopMoveQuest;
    [ReadOnly] public bool pickedup;
    [ReadOnly] public bool idlePickup;
    [ReadOnly] [SerializeField] bool removeAnswer;
    [ReadOnly] [SerializeField] private bool activateQuest;
    [ReadOnly] public List<QuestionItemStageOne> questionList;
    [ReadOnly] public int questCount;
    [ReadOnly] public Vector3 direction;
    [ReadOnly] [SerializeField] private bool firstInitializeData;
    [ReadOnly] public bool gameplayNonInventoryTimerFirstRun;
    [ReadOnly] public bool gameplayInventoryTimerFirstRun;
    [ReadOnly] public bool afterWatchVideo;
    [ReadOnly] public bool pauseTimer;
    [ReadOnly] public int totalScore;
    [ReadOnly] public int totalRightAnswers;
    [ReadOnly] public bool donePopulatingAnswers;
    [ReadOnly] public bool showMap;
    [ReadOnly] [SerializeField] private bool showGradeStats;
    [ReadOnly] [SerializeField] private string gradeStats;

    //  SCENE
    public List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public List<string> scenesLoadingName = new List<string>();

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 30;

        startAtFullAlpha = true;

        genderSetter.GetSetGender = GenderSetter.Gender.NONE;

        sceneController.onSceneChange += LoadingScenes;
        sceneController.onLoadingChange += GameStateOBJEnabler;
        sceneController.onLoadingChange += PlayerEnabler;
        onGameStateChange += GameStateOBJEnabler;
        onGameStateChange += PlayerEnabler;
        inventoryStageOne.onChangeInventoryState += InventoryMenu;
        onPauseStateChange += PauseMenu;

        //  Gravity
        Vector3 gravityS = new Vector3(0, gravityStrength, 0);
        Physics.gravity = gravityS;
    }

    private void Start()
    {
        CheckForFirstInitializeData();
    }

    #region INVENTORY AND PAUSE

    private void InventoryMenu(object sender, EventArgs e)
    {
        if (inventoryStageOne.GetSetInventoryState == InventoryStageOne.InventoryState.INVENTORY)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void PauseMenu(object sender, EventArgs e)
    {
        if (GetSetPauseState == PauseState.NONE)
            Time.timeScale = 1f;
        else if (GetSetPauseState == PauseState.PAUSE || GetSetPauseState == PauseState.SETTINGS)
        {
            if (GetSetGameState == GAMESTATE.GAMEPLAY)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }

    #endregion

    #region SCENE LOADER

    private void LoadingScenes(object sender, EventArgs e)
    {
        StartCoroutine(LoadScene());
    }

    private void GameStateOBJEnabler(object sender, EventArgs e)
    {
        GameControllerEnabler();
        VideoPlayerEnabler();
    }

    private void VideoPlayerEnabler()
    {
        if (GetSetGameState == GAMESTATE.WATCHVIDEO)
        {
            videoPlayerController.gameObject.SetActive(true);

            Time.timeScale = 0f;
        }
        
        if (GetSetGameState == GAMESTATE.GAMEPLAY && videoPlayerController.gameObject.activeSelf)
        {
            videoPlayerController.gameObject.SetActive(false);

            lastGameState = GAMESTATE.NONE;
        }
    }

    private void LoadAnimation()
    {
        if (startAtFullAlpha)
        {
            loadingFadeImage.color = new Color(0, 0, 0, 1);
            startAtFullAlpha = false;
        }
        else
            loadingFadeImage.color = new Color(0, 0, 0, 0);

        loadingCanvas.SetActive(true);

        loadingFade.gameObject.SetActive(true);

        loadingFade.ImageAnimation(loadingFadeImage.color, new Color(0, 0, 0, 1), loadingFade.easeType, loadSpeedAnimation, 0f);
    }

    private void UnLoadAnimation()
    {
        loadingFade.ImageAnimation(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), loadingFade.easeType, loadSpeedAnimation, 
            0f, FinishLoading);
    }

    IEnumerator LoadScene()
    {
        doneLoadingScene = false;

        totalSceneProgress = 0f;
        totalQuestionGeneratedProgress = 0f;
        questionGenerator.progress = 0f;
        questionGenerator.count = 0;

        sceneController.GetSetLoadingStatus = SceneController.LoadingStatus.STARTLOADING;

        LoadAnimation();

        yield return new WaitForSecondsRealtime(loadSpeedAnimation);

        if (sceneController.GetSetPreviousSceneName != "")
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneController.GetSetPreviousSceneName));
            scenesLoadingName.Add(sceneController.GetSetPreviousSceneName);
        }

        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneController.GetSetCurrentSceneName,LoadSceneMode.Additive));
        scenesLoadingName.Add(sceneController.GetSetCurrentSceneName);

        for (int a = 0; a < scenesLoading.Count; a++)
        {
            if (scenesLoadingName[a] == sceneController.GetSetCurrentSceneName)
                scenesLoading[a].allowSceneActivation = false;
        }

        sceneController.GetSetLoadingStatus = SceneController.LoadingStatus.CURRENTLOADING;

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GetSceneLoadProgress()
    {
        loadingPanel.SetActive(true);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1.5f);


        for (int a = 0; a < scenesLoading.Count; a++)
        {
            while (!scenesLoading[a].isDone)
            {
                totalSceneProgress += scenesLoading[a].progress;

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100;

                if (scenesLoading[a].progress >= 0.9f)
                {
                    if (scenesLoadingName[a] == sceneController.GetSetCurrentSceneName)
                    {
                        scenesLoading[a].allowSceneActivation = true;

                        yield return scenesLoading[a];

                        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneController.GetSetCurrentSceneName));
                    }
                    break;
                }
                yield return null;
            }
            yield return null;
        }

        totalSceneProgress = 100;

        doneLoadingScene = true;
        StartCoroutine(GetTotalProgress());
    }

    IEnumerator GetTotalProgress()
    {
        if (GetSetGameState == GAMESTATE.GAMEPLAY)
        {
            totalQuestionGeneratedProgress = 0;

            StartCoroutine(questionGenerator.InstantiateStageOneQuestions());

            while (!questionGenerator.isDone && doneLoadingScene)
            {
                totalQuestionGeneratedProgress = questionGenerator.progress * 100f;

                yield return null;
            }
        }
        else
        {
            totalQuestionGeneratedProgress = 100;
        }

        sceneController.GetSetLoadingStatus = SceneController.LoadingStatus.DONELOADING; 

        Time.timeScale = 1f;

        sceneController.GetSetLoadingStatus = SceneController.LoadingStatus.FINISHLOADING;

        yield return new WaitForSeconds(loadSpeedAnimation);

        loadingPanel.SetActive(false);

        questionGenerator.isDone = false;

        UnLoadAnimation();

        scenesLoading.Clear();
        scenesLoadingName.Clear();
    }

    private void FinishLoading()
    {
        sceneController.GetSetLoadingStatus = SceneController.LoadingStatus.NONE;
        loadingCanvas.SetActive(false);
        loadingFade.gameObject.SetActive(false);
    }

    private void PlayerEnabler(object sender, EventArgs e)
    {
        if (GetSetGameState == GAMESTATE.GAMEPLAY)
        {
            if (sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.FINISHLOADING ||
                sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE)
            {
                foreach (GameObject obj in gameplayObjs)
                    obj.SetActive(true);
            }
            else if (sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.CURRENTLOADING)
            {
                foreach (GameObject obj in gameplayObjs)
                    obj.SetActive(false);
            }
        }

        if (GetSetGameState != GAMESTATE.GAMEPLAY)
        {
            foreach (GameObject obj in gameplayObjs)
                obj.SetActive(false);
        }
    }

    private void GameControllerEnabler()
    {
        if (inventoryStageOne.GetSetInventoryState == InventoryStageOne.InventoryState.NONE && 
            GetSetFindStartState == FindStartState.NONE)
        {
            if (GetSetGameState == GAMESTATE.GAMEPLAY)
            {
                if (sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE && gameControllerCG.alpha == 0f)
                {
                    gameplayControllers.SetActive(true);
                    gameControllerCanvas.CanvasGroupAnimation(1f, gameControllerCanvas.easeType, loadSpeedAnimation, 0f);
                }
                else if (sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.STARTLOADING)
                    gameplayControllers.SetActive(false);
            }
            else
            {
                gameControllerCG.alpha = 0f;
                gameplayControllers.SetActive(false);
            }
        }
    }

    private void CheckForFirstInitializeData()
    {
        if (PlayerPrefs.HasKey("SetFirstInitializeData"))
        {
            GameManager.Instance.soundSystem.GetSetBGVolume = PlayerPrefs.GetFloat("bgvolume");
            GameManager.Instance.soundSystem.GetSetSFXVolume = PlayerPrefs.GetFloat("sfxvolume");

            sceneController.GetSetCurrentSceneName = "MainMenu";
        }
        else
        {
            sceneController.GetSetCurrentSceneName = "InitScene";
        }
    }

    #endregion

    #region PLAYER
    public void PlaceCharacterToSpawnPoint(GameObject character, GameObject spawnPoint)
    {
        character.transform.position = spawnPoint.transform.position;
        character.transform.rotation = spawnPoint.transform.rotation;
    }

    #endregion

    #region VIDEO PLAYER

    #endregion

    #region REUSABLE FUNCTIONS

    public void SaveHighScore(int score, string name, string leaderboardName, string passedFailedStatus)
    {
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name,
            passedFailedStatus = passedFailedStatus };

        //  Load saved Highscores
        string jsonLeaderboardString = PlayerPrefs.GetString(leaderboardName);
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonLeaderboardString);

        //  Add new entry to json
        highscore.highscoreEntryList.Add(highScoreEntry);

        //  Update the json
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString(leaderboardName, json);
        PlayerPrefs.Save();
    }

    public IEnumerator ClearChildList(Transform objTF)
    {
        for (int i = objTF.childCount - 1; i >= 0; i--)
        {
            Destroy(objTF.GetChild(i).gameObject);

            yield return null;
        }
    }

    public IEnumerator StageOneItemList(List<BodyPartItemData> bodyPartList, GameObject instantiateItem, Transform listParent)
    {
        yield return new WaitForEndOfFrame();

        foreach (BodyPartItemData bodypart in bodyPartList)
        {
            GameObject gameobject = Instantiate(instantiateItem, listParent);

            gameobject.GetComponent<BodyPartItem>().bodyPart = bodypart;

            yield return null;
        }
    }

    public void ReturnToMainMenu()
    {
        soundSystem.PlayButton(click);
        sceneController.GetSetCurrentSceneName = "MainMenu";
        soundSystem.timerSourceSFX.Stop();
        GetSetActivateQuestState = false;
        GetSetFindStartState = FindStartState.NONE;
        GetSetPauseState = PauseState.NONE;
        GetSetPickedupState = false;
        GetSetPlayerName = "";
        GetSetRemoveItemState = false;
        GetSetStage = Stage.NONE;
        GetSetTimer = 0f;
        GetSetTutorialState = TutorialState.NONE;
        questionList.Clear();
        StartCoroutine(ClearChildList(answerListObj.transform));
        StartCoroutine(ClearChildList(questionGenerator.questionListTF));
        totalRightAnswers = 0;
        totalScore = 0;
        pauseTimer = false;
        levelGeneratorDifficulty.GetSetDifficulty = Question.Difficulty.NONE;
        levelGeneratorDifficulty.GetSetSubTopic = Question.SubTopic.NONE;
        timerController.GetSetTimerGameplayState = TimerController.TimerGameplayState.NONE;
        inventoryStageOne.GetSetInventoryState = InventoryStageOne.InventoryState.NONE;
        inventoryStageOne.GetBodyPartList().Clear();
        inventoryStageOne.GetSetAnswerBodyPart = null;
        inventoryStageOne.GetSetAnswerBodyPartItem = null;
        inventoryStageOne.GetSetAnswerBodyPartObj = null;
        inventoryStageOne.GetSetBodyPartInventory = null;
        inventoryStageOne.GetSetBodyPartInventoryObj = null;
        inventoryStageOne.GetSetBodyPartItemInventory = null;
        inventoryStageOne.GetSetBodyPartObj = null;
        questionGenerator.selectedQuestionStageOne.Clear();
        genderSetter.GetSetGender = GenderSetter.Gender.NONE;
        genderSetter.GetSetGenderAnimator = null;
        GetSetTutorialState = TutorialState.NONE;
        questCount = 0;
        mainMenu.GetSetMainMenuState = MainMenu.MainMenuState.MAINMENU;
        mainMenu.GetSetSelectedScene = "";
    }

    public void ResetStage()
    {
        soundSystem.PlayButton(click);
        sceneController.GetSetCurrentSceneName = SceneManager.GetActiveScene().name;
        soundSystem.timerSourceSFX.Stop();
        GetSetActivateQuestState = false;
        GetSetFindStartState = FindStartState.NONE;
        GetSetPauseState = PauseState.NONE;
        GetSetPickedupState = false;
        GetSetRemoveItemState = false;
        GetSetStage = Stage.NONE;
        GetSetTimer = 0f;
        StartCoroutine(ClearChildList(answerListObj.transform));
        StartCoroutine(ClearChildList(questionGenerator.questionListTF));
        totalRightAnswers = 0;
        totalScore = 0;
        pauseTimer = false;
        questionGenerator.selectedQuestionStageOne.Clear();
        questionList.Clear();
        timerController.GetSetTimerGameplayState = TimerController.TimerGameplayState.NONE;
        inventoryStageOne.GetBodyPartList().Clear();
        inventoryStageOne.GetSetAnswerBodyPart = null;
        inventoryStageOne.GetSetAnswerBodyPartItem = null;
        inventoryStageOne.GetSetAnswerBodyPartObj = null;
        inventoryStageOne.GetSetBodyPartInventory = null;
        inventoryStageOne.GetSetBodyPartInventoryObj = null;
        inventoryStageOne.GetSetBodyPartItemInventory = null;
        inventoryStageOne.GetSetBodyPartObj = null;
        questCount = 0;
        GetSetTutorialState = TutorialState.NONE;
    }

    public void NextStage()
    {
        soundSystem.PlayButton(click);
        GetSetActivateQuestState = false;
        GetSetFindStartState = FindStartState.NONE;
        GetSetPauseState = PauseState.NONE;
        GetSetPickedupState = false;
        GetSetRemoveItemState = false;
        GetSetStage = Stage.NONE;
        GetSetTimer = 0f;
        StartCoroutine(ClearChildList(answerListObj.transform));
        StartCoroutine(ClearChildList(questionGenerator.questionListTF));
        totalRightAnswers = 0;
        totalScore = 0;
        pauseTimer = false;
        questionGenerator.selectedQuestionStageOne.Clear();
        questionList.Clear();
        timerController.GetSetTimerGameplayState = TimerController.TimerGameplayState.NONE;
        inventoryStageOne.GetBodyPartList().Clear();
        inventoryStageOne.GetSetAnswerBodyPart = null;
        inventoryStageOne.GetSetAnswerBodyPartItem = null;
        inventoryStageOne.GetSetAnswerBodyPartObj = null;
        inventoryStageOne.GetSetBodyPartInventory = null;
        inventoryStageOne.GetSetBodyPartInventoryObj = null;
        inventoryStageOne.GetSetBodyPartItemInventory = null;
        inventoryStageOne.GetSetBodyPartObj = null;
        questCount = 0;
        GetSetTutorialState = TutorialState.NONE;

        if (SceneManager.GetActiveScene().name == "StageOne")
        {
            if (PlayerPrefs.GetInt("StageTwoHard") == 0)
                levelGeneratorDifficulty.GetSetDifficulty = Question.Difficulty.Medium;

            sceneController.GetSetCurrentSceneName = "StageTwo";
        }
    }

    public void OpenMap(bool value)
    {
        GetSetMapState = value;
    }

    #endregion

    public class Highscore
    {
        public List<HighScoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string name;
        public string passedFailedStatus;
    }

}

public static class ThreadSafeRandom
{
    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}

public static class MyExtensions
{
    public static IEnumerator Shuffle<T>(this IList<T> list)
    {

        int n = list.Count;

        while (n > 0)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;

            yield return null;
        }
    }
}
