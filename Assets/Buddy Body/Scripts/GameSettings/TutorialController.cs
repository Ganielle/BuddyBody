using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject easy;
    [SerializeField] private GameObject medium;
    [SerializeField] private GameObject hard;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject maps;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quest;
    [SerializeField] private GameObject timer;

    private void OnEnable()
    {
        easy.SetActive(false);
        medium.SetActive(false);
        hard.SetActive(false);
        controls.SetActive(false);
        maps.SetActive(false);
        inventory.SetActive(false);
        quest.SetActive(false);
        timer.SetActive(false);

        switch (GameManager.Instance.GetSetTutorialState)
        {
            case GameManager.TutorialState.EASYTUTS: easy.SetActive(true); break;
            case GameManager.TutorialState.MEDIUMTUTS: medium.SetActive(true); break;
            case GameManager.TutorialState.HARDTUTS: hard.SetActive(true); break;
            case GameManager.TutorialState.CONTROLSTUTS: controls.SetActive(true); break;
            case GameManager.TutorialState.MAPSTUTS: maps.SetActive(true); break;
            case GameManager.TutorialState.INVENTORYTUTS: inventory.SetActive(true); break;
            case GameManager.TutorialState.QUESTTUTS: quest.SetActive(true); break;
            case GameManager.TutorialState.TIMERTUTS: timer.SetActive(true); break;
            default: break;
        }
    }
}
