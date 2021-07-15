using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isGameplay;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Animator timerAnimator;

    [Header("Debugger")]
    [ReadOnly] public TimerGameplayState timerGameplayState;
    [ReadOnly] [SerializeField] float timeRemaining;
    [ReadOnly] [SerializeField] bool timerIsRunning;
    [ReadOnly] [SerializeField] float minutes;
    [ReadOnly] [SerializeField] float seconds;
    [ReadOnly] [SerializeField] bool checkIfHurryUp;
    [ReadOnly] [SerializeField] bool checkIfTimesUp;
    [ReadOnly] [SerializeField] bool firstRun;


    //  ======================================================
    
    public enum TimerGameplayState 
    {
        NONE,
        COUNTDOWN,
        HURRYUP,
        TIMESUP
    }
    private event EventHandler timerStateChange;
    public event EventHandler onTimerStateChange
    {
        add
        {
            if (timerStateChange == null || !timerStateChange.GetInvocationList().Contains(value))
                timerStateChange += value;
        }
        remove { timerStateChange -= value; }
    }
    public TimerGameplayState GetSetTimerGameplayState
    {
        get { return timerGameplayState; }
        set
        {
            timerGameplayState = value;
            timerStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ===========================================================================

    private void OnEnable()
    {
        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium ||
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            timeRemaining = GameManager.Instance.GetSetTimer;

        timerAnimator.SetBool("HurryUp", false);
        timerAnimator.SetBool("TimesUp", false);

        if (isGameplay)
        {
            if (GameManager.Instance.gameplayNonInventoryTimerFirstRun)
            {
                GetSetTimerGameplayState = TimerGameplayState.COUNTDOWN;

                checkIfHurryUp = true;
                checkIfTimesUp = true;
                timerIsRunning = true;
                GameManager.Instance.gameplayNonInventoryTimerFirstRun = false;
            }
        }
        else
        {
            if (GameManager.Instance.gameplayInventoryTimerFirstRun)
            {
                GetSetTimerGameplayState = TimerGameplayState.COUNTDOWN;

                checkIfHurryUp = true;
                checkIfTimesUp = true;
                timerIsRunning = true;
                GameManager.Instance.gameplayInventoryTimerFirstRun = false;
            }
        }

        if (timeRemaining <= 60)
        {
            timerAnimator.SetBool("HurryUp", true);
            timerAnimator.SetBool("TimesUp", false);
        }
    }

    private void Update()
    {
        if (timerIsRunning && GameManager.Instance.GetSetPauseState == GameManager.PauseState.NONE &&
            GameManager.Instance.GetSetTutorialState == GameManager.TutorialState.NONE && 
            (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium ||
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard) &&
            !GameManager.Instance.pauseTimer && GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
        {
            Timer();
        }
    }

    private void Timer()
    {
        if (isGameplay)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                GameManager.Instance.GetSetTimer = timeRemaining;

                if (checkIfHurryUp && (int)timeRemaining <= 60)
                {
                    checkIfHurryUp = false;
                    
                    if (GetSetTimerGameplayState == TimerGameplayState.COUNTDOWN)
                        GetSetTimerGameplayState = TimerGameplayState.HURRYUP;

                    timerAnimator.SetBool("HurryUp", true);
                    timerAnimator.SetBool("TimesUp", false);
                }
            }
            else
            {
                timeRemaining = 0;
                GameManager.Instance.GetSetTimer = timeRemaining;

                if (checkIfTimesUp)
                {
                    checkIfTimesUp = false;

                    if (GetSetTimerGameplayState == TimerGameplayState.HURRYUP)
                        GetSetTimerGameplayState = TimerGameplayState.TIMESUP;

                    GameManager.Instance.inventoryStageOne.GetSetInventoryState = InventoryStageOne.InventoryState.NONE;
                    timerAnimator.SetBool("TimesUp", true);
                    timerAnimator.SetBool("HurryUp", false);
                }
            }
        }
        else if (!isGameplay)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.unscaledDeltaTime;
                GameManager.Instance.GetSetTimer = timeRemaining;

                if (checkIfHurryUp && (int)timeRemaining <= 60)
                {
                    checkIfHurryUp = false;

                    if (GetSetTimerGameplayState == TimerGameplayState.COUNTDOWN)
                        GetSetTimerGameplayState = TimerGameplayState.HURRYUP;

                    timerAnimator.SetBool("HurryUp", true);
                    timerAnimator.SetBool("TimesUp", false);
                }
            }
            else
            {
                timeRemaining = 0;
                GameManager.Instance.GetSetTimer = timeRemaining;

                if (checkIfTimesUp)
                {
                    checkIfTimesUp = false; 

                    if (GetSetTimerGameplayState == TimerGameplayState.HURRYUP)
                        GetSetTimerGameplayState = TimerGameplayState.TIMESUP;
                    GameManager.Instance.inventoryStageOne.GetSetInventoryState = InventoryStageOne.InventoryState.NONE;
                    timerAnimator.SetBool("TimesUp", true);
                    timerAnimator.SetBool("HurryUp", false);
                }
            }
        }

        DisplayTimer();
    }

    private void DisplayTimer()
    {
        minutes = Mathf.FloorToInt(timeRemaining / 60);
        seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
