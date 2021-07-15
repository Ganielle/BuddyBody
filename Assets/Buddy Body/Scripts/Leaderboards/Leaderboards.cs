using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboards : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardItemUI;

    [Header("GameObject")]
    [SerializeField] private GameObject loadingEasy;
    [SerializeField] private GameObject loadingMedium;
    [SerializeField] private GameObject loadingHard;
    [SerializeField] private GameObject easyNoLeaderboard;
    [SerializeField] private GameObject mediumNoLeaderboard;
    [SerializeField] private GameObject hardNoLeaderboard;
    [SerializeField] private GameObject easyWithLeaderboard;
    [SerializeField] private GameObject mediumWithLeaderboard;
    [SerializeField] private GameObject hardWithLeaderboard;
    [SerializeField] private GameObject passedFailedStatusBoardPanel;

    [Space]
    [SerializeField] private TextMeshProUGUI gradeStats;

    [Header("Transform")]
    [SerializeField] private Transform easyLeaderboardTF;
    [SerializeField] private Transform mediumLeaderboardTF;
    [SerializeField] private Transform hardLeaderboardTF;

    [Header("Button")]
    [SerializeField] private Button stageOneButton;
    [SerializeField] private Button stageTwoButton;

    [Header("Sprite")]
    [SerializeField] private Sprite lightBar;
    [SerializeField] private Sprite darkBar;

    [Header("Sound")]
    [SerializeField] private AudioClip buttonSelect;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] string jsonLeaderboardString;
    [ReadOnly] [SerializeField] LeaderboardScoreStage leaderboardScoreStage;

    private enum LeaderboardScoreStage
    {
        NONE,
        STAGE1,
        STAGE2
    }
    private event EventHandler stageLeaderboardChange;
    private event EventHandler onStageLeaderboardChange
    {
        add
        {
            if (stageLeaderboardChange == null || !stageLeaderboardChange.GetInvocationList().Contains(value))
                stageLeaderboardChange += value;
        }
        remove { stageLeaderboardChange -= value; }
    }
    private LeaderboardScoreStage GetSetLeaderboardScoreStage
    {
        get { return leaderboardScoreStage; }
        set
        {
            leaderboardScoreStage = value;
            stageLeaderboardChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnEnable()
    {
        onStageLeaderboardChange += ChangeLeaderboard;
        GameManager.Instance.onPassFailStatsChange += GradeStatsEnabler;

        GetSetLeaderboardScoreStage = LeaderboardScoreStage.STAGE1;
    }

    private void OnDisable()
    {
        onStageLeaderboardChange -= ChangeLeaderboard;
        GameManager.Instance.onPassFailStatsChange -= GradeStatsEnabler;
    }

    private void GradeStatsEnabler(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetShowGradeStats)
        {
            gradeStats.text = GameManager.Instance.GetSetGradeStats;
            passedFailedStatusBoardPanel.SetActive(true);
        }
        else
        {
            passedFailedStatusBoardPanel.SetActive(false);
            gradeStats.text = "";
            GameManager.Instance.GetSetGradeStats = "";
        }
    }

    private void ChangeLeaderboard(object sender, EventArgs e)
    {
        //  NOTE:
        //  IN NAMING LEADERBORD, STAGE + SCORE + DIFFICULTY
        //  Example: StageOneScoreEasy

        if (GetSetLeaderboardScoreStage == LeaderboardScoreStage.NONE) return;

        else if (GetSetLeaderboardScoreStage == LeaderboardScoreStage.STAGE1)
        {
            StartCoroutine(SetHighScores("StageOneScoreEasy", loadingEasy, easyNoLeaderboard, easyWithLeaderboard, easyLeaderboardTF));
            StartCoroutine(SetHighScores("StageOneScoreMedium", loadingMedium, mediumNoLeaderboard, mediumWithLeaderboard,
                mediumLeaderboardTF));
            StartCoroutine(SetHighScores("StageOneScoreHard", loadingHard, hardNoLeaderboard, hardWithLeaderboard, hardLeaderboardTF));
        }
        else if (GetSetLeaderboardScoreStage == LeaderboardScoreStage.STAGE2)
        {
            StartCoroutine(SetHighScores("StageTwoScoreEasy", loadingEasy, easyNoLeaderboard, easyWithLeaderboard, easyLeaderboardTF));
            StartCoroutine(SetHighScores("StageTwoScoreMedium", loadingMedium, mediumNoLeaderboard, mediumWithLeaderboard,
                mediumLeaderboardTF));
            StartCoroutine(SetHighScores("StageTwoScoreHard", loadingHard, hardNoLeaderboard, hardWithLeaderboard, hardLeaderboardTF));
        }
    }

    IEnumerator SetHighScores(string leaderboardName, GameObject loadingObj, GameObject noLeaderboards, GameObject withLeaderboards,
        Transform leaderboardTF)
    {
        withLeaderboards.SetActive(false);
        loadingObj.SetActive(true);
        noLeaderboards.SetActive(true);

        stageOneButton.interactable = false;
        stageTwoButton.interactable = false;

        yield return GameManager.Instance.ClearChildList(leaderboardTF);

        jsonLeaderboardString = PlayerPrefs.GetString(leaderboardName);
        GameManager.Highscore highscore = JsonUtility.FromJson<GameManager.Highscore>(jsonLeaderboardString);

        if (jsonLeaderboardString != "")
        {
            //  Sort (Bubble sort)
            for (int a = 0; a < highscore.highscoreEntryList.Count; a++)
            {
                for (int b = a + 1; b < highscore.highscoreEntryList.Count; b++)
                {
                    if (highscore.highscoreEntryList[a].score < highscore.highscoreEntryList[b].score)
                    {
                        //  Swap
                        GameManager.HighScoreEntry temp = highscore.highscoreEntryList[a];
                        highscore.highscoreEntryList[a] = highscore.highscoreEntryList[b];
                        highscore.highscoreEntryList[b] = temp;
                    }
                    yield return null;
                }
                yield return null;
            }

            //  Add UI scores
            for (int a = 0; a < highscore.highscoreEntryList.Count; a++)
            {
                GameObject scoreUI = Instantiate(leaderboardItemUI, leaderboardTF);

                LeaderboardsItem itemScript = scoreUI.GetComponent<LeaderboardsItem>();

                //  Sprite bar
                if (a > 1)
                {
                    if (a % 2 == 0)
                        itemScript.barSprite.sprite = darkBar;
                    else
                        itemScript.barSprite.sprite = lightBar;
                }

                itemScript.rankNumber.text = "#" + (a + 1);
                itemScript.scoreNumber.text = Convert.ToString(highscore.highscoreEntryList[a].score);
                itemScript.playerName.text = highscore.highscoreEntryList[a].name;
                itemScript.playerPassFailStats = highscore.highscoreEntryList[a].passedFailedStatus;

                yield return null;
            }
        }


        loadingObj.SetActive(false);

        if (leaderboardTF.childCount > 0)
        {
            noLeaderboards.SetActive(false);
            withLeaderboards.SetActive(true);
        }

        if (GetSetLeaderboardScoreStage == LeaderboardScoreStage.STAGE1)
        {
            stageOneButton.interactable = false;
            stageTwoButton.interactable = true;
        }
        else if (GetSetLeaderboardScoreStage == LeaderboardScoreStage.STAGE2)
        {
            stageOneButton.interactable = true;
            stageTwoButton.interactable = false;
        }
    }

    public void StageOneButtonScoreBoard()
    {
        GameManager.Instance.soundSystem.PlayButton(buttonSelect);
        GetSetLeaderboardScoreStage = LeaderboardScoreStage.STAGE1;
    }

    public void StageTwoScoreBoard()
    {
        GameManager.Instance.soundSystem.PlayButton(buttonSelect);
        GetSetLeaderboardScoreStage = LeaderboardScoreStage.STAGE2;
    }

    public void BackPassFailStatsPanel()
    {
        GameManager.Instance.soundSystem.PlayButton(buttonSelect);
        GameManager.Instance.GetSetShowGradeStats = false;
    }
}
