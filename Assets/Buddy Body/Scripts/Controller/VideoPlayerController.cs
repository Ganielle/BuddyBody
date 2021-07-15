using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [Header("LeanTween")]
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float speed;
    [SerializeField] private float delay;

    [Header("CanvasGroup")]
    [SerializeField] private CanvasGroup videoCanvasGroup;
    [SerializeField] private CanvasGroup playPausePanel;

    [Header("Settings")]
    public VideoPlayer videoPlayer;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;

    private void OnEnable()
    {
        SetVideo();

        videoCanvasGroup.alpha = 0f;

        playPausePanel.gameObject.SetActive(true);

        LeanTween.alphaCanvas(videoCanvasGroup, 1f, speed).setDelay(delay).setEase(easeType);

        ShowPlayPausePanel();
    }

    private void OnDisable()
    {
        videoPlayer.clip = null;
    }

    private void SetVideo()
    {
        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory != null)
            videoPlayer.clip = GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory.bodyPart.bodyPart.bodyPartVideoClip;
        else
            videoPlayer.clip = null;

        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        LeanTween.alphaCanvas(playPausePanel, 0f, speed).setEase(easeType).setDelay(delay).setOnComplete(() => playPausePanel.gameObject.SetActive(false));
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void CloseVideo()
    {
        LeanTween.alphaCanvas(videoCanvasGroup, 0f, speed).setDelay(delay).setEase(easeType).setOnComplete(() => 
        GameManager.Instance.GetSetGameState = GameManager.Instance.lastGameState);
    }

    public void ShowPlayPausePanel() => StartCoroutine(PlayPausePanel());

    IEnumerator PlayPausePanel()
    {
        playPausePanel.alpha = 0f;

        playPausePanel.gameObject.SetActive(true);

        LeanTween.alphaCanvas(playPausePanel, 1f, speed).setEase(easeType).setDelay(delay);

        if (videoPlayer.isPlaying)
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);

            yield return new WaitForSecondsRealtime(2f);

            LeanTween.alphaCanvas(playPausePanel, 0f, speed).setEase(easeType).setDelay(delay).setOnComplete(() => playPausePanel.gameObject.SetActive(false));
        }
        else
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
        }
    }
}
