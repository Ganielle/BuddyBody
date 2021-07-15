using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject questItem;
    [SerializeField] private float backForce;
    [SerializeField] private GameObject itemAreaCollider;
    [SerializeField] private Collider capsuleCollider;

    [Header("Audio")]
    [SerializeField] private AudioClip questBGAudioClip;
    [SerializeField] private AudioClip questFinishClip;
    [SerializeField] private float maxTime;
    [SerializeField] private float loopTime;

    [Header("Script")]
    [SerializeField] private ItemLocatorController itemLocatorController;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] bool setQuests;
    [ReadOnly] [SerializeField] AudioClip lastStageBG;
    [ReadOnly] [SerializeField] float lastMaxTime;
    [ReadOnly] [SerializeField] float lastLoopTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && setQuests)
        {
            GameManager.Instance.GetSetActivateQuestState = true;

            if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.COUNTDOWN ||
                GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.NONE)
            {
                lastStageBG = GameManager.Instance.soundSystem.bgAudioSource.clip;

                lastMaxTime = GameManager.Instance.soundSystem.GetMaxTime;
                lastLoopTime = GameManager.Instance.soundSystem.GetLoopTime;

                GameManager.Instance.soundSystem.ChangeBGMusic(questBGAudioClip, maxTime, loopTime);
            }

            setQuests = false;
            itemAreaCollider.SetActive(true);
            StartCoroutine(QuestSetter(itemLocatorController.GetItemList));
        }
    }

    private void OnEnable()
    {
        itemAreaCollider.SetActive(false);

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy || 
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium)
            setQuests = true; 
        
        else if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            capsuleCollider.enabled = false;

        itemLocatorController.onChangeItemList += CheckActiveQuest;
    }

    private void OnDisable()
    {
        GameManager.Instance.GetSetActivateQuestState = false;
        itemLocatorController.onChangeItemList -= CheckActiveQuest;
    }

    private void CheckActiveQuest(object sender, EventArgs e)
    {
        StartCoroutine(PlayBGMusicAgain());
    }

    IEnumerator PlayBGMusicAgain()
    {

        if (itemLocatorController.GetItemList.Count == 0 && (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy ||
            GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Medium))
        {
            GameManager.Instance.soundSystem.PlayQuestSFX(questFinishClip);

            itemAreaCollider.SetActive(false);

            GameManager.Instance.soundSystem.bgAudioSource.Stop();

            if (GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.COUNTDOWN ||
                GameManager.Instance.timerController.GetSetTimerGameplayState == TimerController.TimerGameplayState.NONE)
            {
                GameManager.Instance.GetSetActivateQuestState = false;

                yield return new WaitForSecondsRealtime(8.23f);

                GameManager.Instance.soundSystem.ChangeBGMusic(lastStageBG, lastMaxTime, lastLoopTime);
            }
        }
    }

    public IEnumerator QuestSetter(List<GameObject> itemListCount)
    {
        for (int a = 0; a < itemListCount.Count; a++)
        {
            //  UI QUEST ITEM
            GameObject bodyPartQuest = Instantiate(questItem, GameManager.Instance.questListObj.transform);

            bodyPartQuest.GetComponent<QuestUIController>().bodyPart.bodyPart = itemLocatorController.GetItemList[a].gameObject.
                GetComponent<BodyPartController>().bodyPart.bodyPart;
            bodyPartQuest.GetComponent<QuestUIController>().bodyPart.uniqueID = itemLocatorController.GetItemList[a].gameObject.
                GetComponent<BodyPartController>().bodyPart.uniqueID;

            yield return null;
        }
    }
}
