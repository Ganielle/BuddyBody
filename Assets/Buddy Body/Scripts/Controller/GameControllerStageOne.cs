using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerStageOne : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mediumMaxTimer;
    [SerializeField] private float hardMaxTimer;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject checkingCamera;
    [SerializeField] private GameObject overviewMapCamera;

    [Header("Body Parts")]
    [SerializeField] private List<BodyPart> externalUpperBodyPartData;
    [SerializeField] private List<BodyPart> externalLowerBodyPartData;
    [SerializeField] private GameObject itemLocatorOne;
    [SerializeField] private GameObject itemLocatorTwo;
    [SerializeField] private GameObject itemLocatorThree;
    [SerializeField] private GameObject bodyPartFieldItem;

    [Header("SpawnPoints")]
    [SerializeField] private GameObject characterSpawnPoints;
    [SerializeField] private List<GameObject> bodyPartSpawnPointsOne;
    [SerializeField] private List<GameObject> bodyPartSpawnPointsTwo;
    [SerializeField] private List<GameObject> bodyPartSpawnPointsThree;

    [Header("Audio")]
    [SerializeField] private AudioClip hurryUpClip;
    [SerializeField] private AudioClip HurryUpTimerClip;
    [SerializeField] private AudioClip questBG;
    [SerializeField] private AudioClip timesUpClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip questStartClip;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] int randomSpawnChoice;
    [ReadOnly] [SerializeField] bool firstInstantiate;
    [ReadOnly] [SerializeField] bool firstPopulate;

    private void OnEnable()
    {
        firstPopulate = true;
        GameManager.Instance.gameplayNonInventoryTimerFirstRun = true;
        GameManager.Instance.gameplayInventoryTimerFirstRun = true;
        firstInstantiate = true;

        GameManager.Instance.overviewMap = overviewMapCamera;

        StartCoroutine(GameManager.Instance.ClearChildList(GameManager.Instance.questListObj.transform));
        GameManager.Instance.timerController.GetSetTimerGameplayState = TimerController.TimerGameplayState.NONE;

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium)
            GameManager.Instance.GetSetTimer = mediumMaxTimer;
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            GameManager.Instance.GetSetTimer = hardMaxTimer;

        GameManager.Instance.sceneController.onLoadingChange += OnGameStateChange;
        GameManager.Instance.onGameStateChange += OnGameStateChange;
        GameManager.Instance.onTutorialStateChange += OnGameStateChange;
        GameManager.Instance.timerController.onTimerStateChange += CheckGameplayTimer;
        GameManager.Instance.onActivateQuestChange += ActivateQuest;
    }

    private void OnDisable()
    {
        GameManager.Instance.sceneController.onLoadingChange -= OnGameStateChange;
        GameManager.Instance.onGameStateChange -= OnGameStateChange;
        GameManager.Instance.onTutorialStateChange -= OnGameStateChange;
        GameManager.Instance.timerController.onTimerStateChange-= CheckGameplayTimer;
        GameManager.Instance.onActivateQuestChange -= ActivateQuest;
    }

    private void Start()
    {
        GameManager.Instance.PlaceCharacterToSpawnPoint(GameManager.Instance.playerObj, characterSpawnPoints);
    }

    private void ActivateQuest(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetActivateQuestState)
            GameManager.Instance.soundSystem.PlayQuestSFX(questStartClip);
    }

    private void OnGameStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE &&
            GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY &&
            GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.NONE &&
            GameManager.Instance.GetSetFindStartState == GameManager.FindStartState.NONE && firstInstantiate)
        {
            InstantiateBodyItems();
            firstInstantiate = false;
        }


        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEOVER)
        {
            GameManager.Instance.soundSystem.ChangeBGMusic(gameOverClip, 105f, 0f);
            Time.timeScale = 0f;
        }

        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CHECKING && firstPopulate)
        {
            GameManager.Instance.soundSystem.timerSourceSFX.Stop();
            firstPopulate = false;
            StartCoroutine(SpawnAnswers());
            StartCoroutine(CheckingCamera());
        }
    }

    #region ITEM SPAWN POINTS

    private void InstantiateBodyItems()
    {
        randomSpawnChoice = UnityEngine.Random.Range(1,3);

        if (randomSpawnChoice == 3)
            randomSpawnChoice = 2;

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetSubTopic == Question.SubTopic.UPPER)
        {
            if (randomSpawnChoice == 1)
                StartCoroutine(BodyParts(bodyPartSpawnPointsOne, externalUpperBodyPartData, itemLocatorOne));
            else if (randomSpawnChoice == 2)
                StartCoroutine(BodyParts(bodyPartSpawnPointsTwo, externalUpperBodyPartData, itemLocatorTwo));
            else if (randomSpawnChoice == 3)
                StartCoroutine(BodyParts(bodyPartSpawnPointsThree, externalUpperBodyPartData, itemLocatorThree));
        }
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetSubTopic == Question.SubTopic.LOWER)
        {
            if (randomSpawnChoice == 1)
                StartCoroutine(BodyParts(bodyPartSpawnPointsOne, externalLowerBodyPartData, itemLocatorOne));
            else if (randomSpawnChoice == 2)
                StartCoroutine(BodyParts(bodyPartSpawnPointsTwo, externalLowerBodyPartData, itemLocatorTwo));
            else if (randomSpawnChoice == 3)
                StartCoroutine(BodyParts(bodyPartSpawnPointsThree, externalLowerBodyPartData, itemLocatorThree));
        }
    }

    IEnumerator BodyParts(List<GameObject> spawnPoints, List<BodyPart> bodyPartData, GameObject itemLocator)
    {
        for (int a = 0; a < spawnPoints.Count; a++)
        {
            GameObject bodypart = Instantiate(bodyPartFieldItem, spawnPoints[a].transform);

            bodypart.transform.position = spawnPoints[a].transform.position;

            bodypart.GetComponent<BodyPartController>().bodyPart.bodyPart = bodyPartData[a];

            yield return null;
        }

        itemLocator.SetActive(true);

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
        {
            GameManager.Instance.GetSetActivateQuestState = true;

            GameManager.Instance.soundSystem.ChangeBGMusic(questBG, 1800, 0);
        }
    }

    #endregion

    #region TIMER GAMEPLAY

    private void CheckGameplayTimer(object sender, EventArgs e)
    {
        PlayHurryUpPlusBGMusic();
    }

    private void PlayHurryUpPlusBGMusic()
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
        {
            if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.HURRYUP)
            {
                GameManager.Instance.soundSystem.PlayGameplayTimerSFX(hurryUpClip, false);

                if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy || 
                    GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium)
                    GameManager.Instance.soundSystem.ChangeBGMusic(questBG, 1800, 0);

                GameManager.Instance.soundSystem.PlayGameplayTimerSFX(HurryUpTimerClip, true);
            }
            else if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.TIMESUP)
            {
                GameManager.Instance.soundSystem.timerSourceSFX.Stop();
                GameManager.Instance.soundSystem.timerSourceSFX.clip = null;

                GameManager.Instance.soundSystem.timerSourceSFX.PlayOneShot(timesUpClip);

                GameManager.Instance.soundSystem.ChangeBGMusic(null, 0, 0);
            }
        }
    }

    #endregion

    #region CHECKING ANSWERS

    IEnumerator SpawnAnswers()
    {
        GameManager.Instance.donePopulatingAnswers = false;

        for (int a = 0; a < GameManager.Instance.questionGenerator.questionListTF.childCount; a++)
        {
            Instantiate(GameManager.Instance.questionGenerator.questionListTF.GetChild(a).gameObject,
                GameManager.Instance.answerListObj.transform);

            yield return null;
        }

        GameManager.Instance.donePopulatingAnswers = true;
    }

    IEnumerator CheckingCamera()
    {
        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 1f;

        GameManager.Instance.playerObj.SetActive(false);
        playerCamera.SetActive(false);
        checkingCamera.SetActive(true);
    }

    #endregion
}
