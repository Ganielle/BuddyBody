using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenu
{
    public enum MainMenuState
    {
        NONE,
        MAINMENU,
        STAGESELECT,
        DIFFICULTYSELECT,
        GENDERSELECT,
        LEADERBOARDS,
        QUITGAME
    }
    private event EventHandler changeMainMenuState;
    public event EventHandler onChangeMainMenuState
    {
        add
        {
            if (changeMainMenuState == null || !changeMainMenuState.GetInvocationList().Contains(value))
                changeMainMenuState += value;
        }
        remove { changeMainMenuState -= value; }
    }
    MainMenuState mainMenuState;
    public MainMenuState GetSetMainMenuState
    {
        get { return mainMenuState; }
        set
        {
            mainMenuState = value;
            changeMainMenuState?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler changeStageSelected;
    public event EventHandler onChangeStageSelected
    {
        add
        {
            if (changeStageSelected == null || !changeStageSelected.GetInvocationList().Contains(value))
                changeStageSelected += value;
        }
        remove { changeStageSelected -= value; }
    }
    string selectedScene;
    public string GetSetSelectedScene
    {
        get { return selectedScene; }
        set
        {
            selectedScene = value;
            changeStageSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
