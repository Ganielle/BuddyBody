using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Button retryButton;

    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;

    private void OnEnable()
    {
        returnToMainMenuButton.interactable = true;
        retryButton.interactable = true;
    }

    public void ChangeToGameOverState() => GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.GAMEOVER;

    public void ReturnToMainMenu()
    {
        returnToMainMenuButton.interactable = false;
        retryButton.interactable = false;
    }

    public void RetryButton()
    {
        returnToMainMenuButton.interactable = false;
        retryButton.interactable = false;
    }
}
