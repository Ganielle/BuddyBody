using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneController
{
    public enum LoadingStatus
    {
        NONE,
        STARTLOADING,
        CURRENTLOADING,
        DONELOADING,
        FINISHLOADING
    }

    private event EventHandler loadingChange;
    public event EventHandler onLoadingChange
    {
        add
        {
            if (loadingChange == null || !loadingChange.GetInvocationList().Contains(value))
                loadingChange += value;
        }
        remove
        {
            loadingChange -= value;
        }
    }
    LoadingStatus loadingStatus;
    public LoadingStatus GetSetLoadingStatus
    {
        get { return loadingStatus; }
        set
        { 
            loadingStatus = value;
            loadingChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler sceneChange;
    public event EventHandler onSceneChange
    {
        add
        {
            if (sceneChange == null || !sceneChange.GetInvocationList().Contains(value))
                sceneChange += value;
        }
        remove
        {
            sceneChange -= value;
        }
    }
    string currentSceneName = "", previousSceneName = "";
    public string GetSetCurrentSceneName
    {
        get { return currentSceneName; }
        set
        {
            if (!currentSceneName.Equals(""))
                previousSceneName = currentSceneName;

            currentSceneName = value;
            sceneChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public string GetSetPreviousSceneName
    {
        get { return previousSceneName; }
        set { previousSceneName = value; }
    }
}
