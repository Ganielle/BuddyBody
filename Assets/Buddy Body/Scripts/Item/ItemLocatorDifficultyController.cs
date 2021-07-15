using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLocatorDifficultyController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Image> itemLocator;

    private void OnEnable()
    {
        Locators();
    }

    private void Locators()
    {
        if (GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY)
        {
            if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            {
                foreach (Image image in itemLocator)
                    image.enabled = false;
            }
            else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy ||
                GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium)
            {
                foreach (Image image in itemLocator)
                    image.enabled = true;
            }
        }
    }
}
