using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Gameobjects")]
    [SerializeField] private GameObject gamepadController;
    [SerializeField] private GameObject stageOneBackpack;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject startCountdownTimer;
    [SerializeField] private GameObject GameoverPanel;
    [SerializeField] private GameObject itemListContentGO;
    [SerializeField] private GameObject checkingAnswerObj;
    [SerializeField] private GameObject overviewMapPanel;
    [SerializeField] private GameObject tutorialPanel;

    private void Start()
    {
        GameManager.Instance.onGameStateChange += OnGameStateChange;
        GameManager.Instance.inventoryStageOne.onChangeInventoryState += InventoryEnabler;
        GameManager.Instance.onPauseStateChange += PauseMenu;
        GameManager.Instance.onFindStartStateChange += CountdownTimer;
        GameManager.Instance.timerController.onTimerStateChange += GameOver;
        GameManager.Instance.mapStateChange += Map;
        GameManager.Instance.onTutorialStateChange += TutorialStateChange;
    }

    private void TutorialStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.NONE)
            return;

        tutorialPanel.SetActive(true);
    }

    private void OnGameStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CHECKING)
        {
            GameManager.Instance.soundSystem.timerSourceSFX.Stop();
            gamepadController.SetActive(false);
            GameManager.Instance.inventoryStageOne.GetSetInventoryState = InventoryStageOne.InventoryState.NONE;
            checkingAnswerObj.SetActive(true);
        }
        else
        {
            checkingAnswerObj.SetActive(false);
        }
    }

    private void GameOver(object sender, EventArgs e)
    {
        if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.TIMESUP)
        {
            gamepadController.SetActive(false);
            stageOneBackpack.SetActive(false);
            GameoverPanel.SetActive(true);
        }
        else if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.NONE)
            GameoverPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.Instance.inventoryStageOne.onChangeInventoryState -= InventoryEnabler;
        GameManager.Instance.onPauseStateChange -= PauseMenu;
        GameManager.Instance.mapStateChange -= Map;
        GameManager.Instance.onFindStartStateChange -= CountdownTimer;
    }

    private void Map(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetMapState)
        {
            Time.timeScale = 0f;
            gamepadController.SetActive(false);
            overviewMapPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            overviewMapPanel.SetActive(false);
            gamepadController.SetActive(true);
        }
    }

    private void CountdownTimer(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetFindStartState == GameManager.FindStartState.STARTCOUNTDOWN)
            startCountdownTimer.SetActive(true);
    }

    #region INVENTORY

    private void InventoryEnabler(object sender, EventArgs e)
    {
        InventoryChecker();
    }

    private void InventoryChecker()
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY && 
            GameManager.Instance.timerController.GetSetTimerGameplayState != TimerController.TimerGameplayState.TIMESUP)
        {
            if (GameManager.Instance.inventoryStageOne.GetSetInventoryState == InventoryStageOne.InventoryState.INVENTORY)
            {
                GameManager.Instance.direction = new Vector3(0f, 0f, 0f);
                gamepadController.SetActive(false);
                stageOneBackpack.SetActive(true);
            }
            else
            {
                stageOneBackpack.SetActive(false);
                gamepadController.SetActive(true);
                StartCoroutine(GameManager.Instance.ClearChildList(itemListContentGO.transform));
            }
        }
        
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CHECKING &&
            GameManager.Instance.inventoryStageOne.GetSetInventoryState == InventoryStageOne.InventoryState.NONE)
        {
            GameManager.Instance.direction = new Vector3(0f, 0f, 0f);
            StartCoroutine(GameManager.Instance.ClearChildList(itemListContentGO.transform));
            stageOneBackpack.SetActive(false);
        }
    }

    #endregion

    #region PAUSE MENU

    private void PauseMenu(object sender, EventArgs e)
    {
        PauseMenuEnabler();
    }

    private void PauseMenuEnabler()
    {
        if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.NONE)
        {
            if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
                Time.timeScale = 1f;

            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
        }
        else if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.PAUSE)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
        else if (GameManager.Instance.GetSetPauseState == GameManager.PauseState.SETTINGS)
        {
            if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
                Time.timeScale = 0f;

            pauseMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }

    #endregion
}
