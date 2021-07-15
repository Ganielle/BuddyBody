using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSetter : MonoBehaviour
{
    [SerializeField] private bool changeGameStateOnStart;
    [ConditionalField("changeGameStateOnStart")] [SerializeField] private GameManager.GAMESTATE gameState;

    private void Awake()
    {
        if (changeGameStateOnStart)
            GameManager.Instance.GetSetGameState = gameState;
    }

    public void SetCutsceneGameState() => GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.CUTSCENE;

    public void SetGameplayGameState() => GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.GAMEPLAY;

    public void CheckForTutorial()
    {
        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy &&
           PlayerPrefs.GetInt("easyTutorial") == 0)
        {
            GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.EASYTUTS;
            Debug.Log("easy tutorial");
        }
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy &&
           PlayerPrefs.GetInt("easyTutorial") == 1)
        {
            GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.STARTCOUNTDOWN;
        }

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium &&
            PlayerPrefs.GetInt("mediumTutorial") == 0)
        {
            GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.MEDIUMTUTS;

            Debug.Log("medium tutorial");
        }
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium &&
            PlayerPrefs.GetInt("mediumTutorial") == 1)
        {
            GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.STARTCOUNTDOWN;
        }

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard &&
            PlayerPrefs.GetInt("hardTutorial") == 0)
        {
            GameManager.Instance.GetSetTutorialState = GameManager.TutorialState.HARDTUTS;
            Debug.Log("hard tutorial");
        }
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard &&
            PlayerPrefs.GetInt("hardTutorial") == 1)
        {
            GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.STARTCOUNTDOWN;
        }

        //if (value)
        //{
        //    GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.STARTCOUNTDOWN;
        //}
        //else
        //{
        //    GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.NONE;
        //}
    }
}
