using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableDirectorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayableAsset startCutscene;
    [SerializeField] private PlayableDirector playableDirector;

    private void OnEnable()
    {
        GameManager.Instance.onGameStateChange += ChangeTimelineAsset;
        GameManager.Instance.sceneController.onLoadingChange += PlayDirector;
    }

    private void OnDisable()
    {

        GameManager.Instance.onGameStateChange -= ChangeTimelineAsset;
        GameManager.Instance.sceneController.onLoadingChange -= PlayDirector;
    }

    private void PlayDirector(object sender, EventArgs e)
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.FINISHLOADING)
        {
            playableDirector.playableAsset = startCutscene;

            playableDirector.RebuildGraph();
            playableDirector.time = 0.0;
            playableDirector.Play();
        }
    }

    private void ChangeTimelineAsset(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
        {
            playableDirector.playableAsset = null;

            playableDirector.RebuildGraph();
            playableDirector.time = 0.0;
        }
    }
}
