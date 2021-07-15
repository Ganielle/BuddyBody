using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource bgAudioSource;
    [SerializeField] private AudioSource stageAmbianceSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource buttonSource;
    [SerializeField] private AudioSource questSource;
    public AudioSource countdownTimer;
    public AudioSource timerSourceSFX;

    [Space]
    [SerializeField] private bool debugMode;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] private float bgVolume;
    [ReadOnly] [SerializeField] private float sfxVolume;
    [ReadOnly] [SerializeField] private float maxTime;
    [ReadOnly] [SerializeField] private float loopTime;
    [ReadOnly] public bool shouldLoopMusic;

    // =======================================================

    private event EventHandler bgVolumeChange;
    public event EventHandler onBGVolumeChange
    {
        add
        {
            if (bgVolumeChange == null || !bgVolumeChange.GetInvocationList().Contains(value))
                bgVolumeChange += value;
        }
        remove
        {
            bgVolumeChange -= value;
        }
    }
    public float GetSetBGVolume
    {
        get { return bgVolume; }
        set
        {
            bgVolume = value;
            bgVolumeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler sfxVolumeChange;
    public event EventHandler onSFXVolumeChange
    {
        add
        {
            if (sfxVolumeChange == null || !sfxVolumeChange.GetInvocationList().Contains(value))
                sfxVolumeChange += value;
        }
        remove
        {
            sfxVolumeChange -= value;
        }
    }
    public float GetSetSFXVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            sfxVolumeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetLoopTime
    {
        get { return loopTime; }
    }

    public float GetMaxTime
    {
        get { return maxTime; }
    }

    //  ======================================================

    private void OnEnable()
    {
        onBGVolumeChange += ChangeBGVolume;
        onSFXVolumeChange += ChangeSFXVolume;

    }

    private void OnDisable()
    {
        onBGVolumeChange += ChangeBGVolume;
        onSFXVolumeChange += ChangeSFXVolume;
        GameManager.Instance.onActivateQuestChange -= QuestMusic;
        GameManager.Instance.onGameStateChange -= PauseMusics;
    }

    private void Start()
    {
        GameManager.Instance.sceneController.onLoadingChange += FadeBGMusic;
        GameManager.Instance.onActivateQuestChange += QuestMusic;
        GameManager.Instance.onGameStateChange += PauseMusics;

        if (debugMode)
        {
            GetSetBGVolume = 1f;
            GetSetSFXVolume = 1f;
        }
        else
        {
            SetBGVolume();
            SetSFXVolume();
        }
    }

    private void LateUpdate()
    {
        BGMusicLooper();
    }

    private void PauseMusics(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.WATCHVIDEO)
        {
            StartCoroutine(MusicVolumeChange(bgAudioSource, 0f, 0.25f, true));
            StartCoroutine(MusicVolumeChange(timerSourceSFX, 0f, 0.25f, true));
        }
        
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY && GameManager.Instance.afterWatchVideo)
        {
            GameManager.Instance.afterWatchVideo = false;
            StartCoroutine(MusicVolumeChange(bgAudioSource, GetSetBGVolume, 0.25f, true));
            StartCoroutine(MusicVolumeChange(timerSourceSFX, GetSetSFXVolume, 0.25f, true));
        }
    }


    private void ChangeBGVolume(object sender, EventArgs e)
    {
        bgAudioSource.volume = GetSetBGVolume;

        PlayerPrefs.SetFloat("bgvolume", GetSetBGVolume);
    }

    private void ChangeSFXVolume(object sender, EventArgs e)
    {
        SetSFXVolume();

        PlayerPrefs.SetFloat("sfxvolume", GetSetSFXVolume);
    }

    private void QuestMusic(object sender, EventArgs e)
    {
        StartCoroutine(ChangeMusicQuest());
    }

    private void SetSFXVolume()
    {
        stageAmbianceSource.volume = GetSetSFXVolume;
        sfxSource.volume = GetSetSFXVolume;
        buttonSource.volume = GetSetSFXVolume;
        questSource.volume = GetSetSFXVolume;
        countdownTimer.volume = GetSetSFXVolume;
        timerSourceSFX.volume = GetSetSFXVolume;
    }

    private void SetBGVolume() => bgAudioSource.volume = GetSetBGVolume;

    IEnumerator ChangeMusicQuest()
    {
        StartCoroutine(MusicVolumeChange(bgAudioSource, 0f, 1f, true));

        yield return new WaitForSeconds(1f);

        StartCoroutine(MusicVolumeChange(bgAudioSource, GetSetBGVolume, 1f, true));
    }

    private void FadeBGMusic(object sender, EventArgs e)
    {
        if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.STARTLOADING)
        {
            StartCoroutine(MusicVolumeChange(bgAudioSource, 0f, 1f, false));
            StartCoroutine(MusicVolumeChange(stageAmbianceSource, 0f, 1f, false));

        }
        else if (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.FINISHLOADING)
        {
            StartCoroutine(MusicVolumeChange(bgAudioSource, GetSetBGVolume, 1f, false));
            StartCoroutine(MusicVolumeChange(stageAmbianceSource, GetSetSFXVolume, 1f, false));
        }
    }

    private void BGMusicLooper()
    {
        if (bgAudioSource.time >= maxTime && 
            GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE &&
            shouldLoopMusic)
        {
            bgAudioSource.time = Convert.ToInt32(loopTime);
            bgAudioSource.Play();
        }
    }

    IEnumerator MusicVolumeChange(AudioSource audioSource, float targetVolume, float duration, bool isRealTime)
    {
        float currentTime = 0;
        float start = bgAudioSource.volume;

        while (currentTime < duration)
        {
            if (isRealTime)
                currentTime += Time.unscaledDeltaTime;
            else
                currentTime += Time.unscaledDeltaTime;

            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);

            yield return null;
        }
    }

    public void ChangeBGMusic(AudioClip clip, float maxTime, float loopTime)
    {
        bgAudioSource.time = 0f;
        bgAudioSource.clip = clip;
        this.maxTime = maxTime;
        this.loopTime = loopTime;
        bgAudioSource.Play();
    }

    public void ChangeAmbianceSound(AudioClip clip)
    {
        stageAmbianceSource.time = 0f;
        stageAmbianceSource.clip = clip;
        stageAmbianceSource.Play();
    }

    public void PlaySFX(AudioClip clip) => sfxSource.PlayOneShot(clip);

    public void PlayButton(AudioClip clip) => buttonSource.PlayOneShot(clip);

    public void PlayQuestSFX(AudioClip clip) => questSource.PlayOneShot(clip);

    public void PlayCountDownTimer(AudioClip clip) => countdownTimer.PlayOneShot(clip);

    public void PlayGameplayTimerSFX(AudioClip clip, bool value)
    {
        if (value)
        {
            timerSourceSFX.clip = clip;
            timerSourceSFX.loop = true;
            timerSourceSFX.Play();
        }
        else
        {
            timerSourceSFX.PlayOneShot(clip);
        }
    }
}
