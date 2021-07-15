using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clickButtonClip;
    [SerializeField] private AudioClip closePauseClip;

    [Header("Gameobjects")]
    [SerializeField] private GameObject areYouSureObj;

    private void OnEnable()
    {
        areYouSureObj.SetActive(false);
    }

    public void ResumeButton()
    {
        GameManager.Instance.soundSystem.PlayButton(closePauseClip);
        GameManager.Instance.GetSetPauseState = GameManager.PauseState.NONE;
    }

    public void SettingsMenuButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        GameManager.Instance.GetSetPauseState = GameManager.PauseState.SETTINGS;
    }

    public void ReturnToMainMenuAreYouSure()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        areYouSureObj.SetActive(true);
    }

    public void YesButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
    }

    public void NoButton()
    {
        GameManager.Instance.soundSystem.PlayButton(clickButtonClip);
        areYouSureObj.SetActive(false);
    }
}
