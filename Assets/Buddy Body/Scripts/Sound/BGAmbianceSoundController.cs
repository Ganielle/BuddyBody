using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAmbianceSoundController : MonoBehaviour
{
    [SerializeField] private bool bgMusic;
    [SerializeField] private bool ambianceMusic;
    [SerializeField] private bool playOnAwake;

    [Header("BG Music")]
    [ConditionalField("playOnAwake", true)][SerializeField] private AudioClip beforeFindItemsClip;
    [ConditionalField("bgMusic")] [SerializeField] private AudioClip bgMusicClip;
    [ConditionalField("bgMusic")] [SerializeField] float maxTime;
    [ConditionalField("bgMusic")] [SerializeField] float loopTime;

    [Header("Ambiance Music")]
    [ConditionalField("ambianceMusic")] [SerializeField] private AudioClip ambianceMusicClip;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] bool playOnce;

    private void Awake()
    {
        playOnce = true;
    }

    private void OnEnable()
    {
        GameManager.Instance.onGameStateChange += ChangeBGMusic;
        GameManager.Instance.onFindStartStateChange += ChangeBGMusic;
    }

    private void OnDisable()
    {
        GameManager.Instance.onGameStateChange -= ChangeBGMusic;
        GameManager.Instance.onFindStartStateChange -= ChangeBGMusic;
    }

    private void ChangeBGMusic(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.CUTSCENE && playOnce)
        {
            playOnce = false;
            GameManager.Instance.soundSystem.shouldLoopMusic = false;
            GameManager.Instance.soundSystem.bgAudioSource.clip = null;
            GameManager.Instance.soundSystem.bgAudioSource.PlayOneShot(beforeFindItemsClip);
        }

        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY &&
            GameManager.Instance.GetSetFindStartState == GameManager.FindStartState.NONE &&
            !GameManager.Instance.GetSetActivateQuestState)
        {
            GameManager.Instance.soundSystem.shouldLoopMusic = true;

            if (bgMusic)
                GameManager.Instance.soundSystem.ChangeBGMusic(bgMusicClip, maxTime, loopTime);
            else
                GameManager.Instance.soundSystem.ChangeBGMusic(null, 0f, 0f);
        }
    }

    private void Start()
    {
        if (playOnAwake)
        {
            GameManager.Instance.soundSystem.shouldLoopMusic = true;
            if (bgMusic)
                GameManager.Instance.soundSystem.ChangeBGMusic(bgMusicClip, maxTime, loopTime);
            else
                GameManager.Instance.soundSystem.ChangeBGMusic(null, 0f, 0f);
        }

        if (ambianceMusic)
            GameManager.Instance.soundSystem.ChangeAmbianceSound(ambianceMusicClip);
        else
            GameManager.Instance.soundSystem.ChangeAmbianceSound(null);
    }
}
