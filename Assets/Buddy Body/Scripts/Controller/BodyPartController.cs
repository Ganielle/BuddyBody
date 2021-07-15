using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class BodyPartController : MonoBehaviour
{
    public BodyPartItemData bodyPart;
    public GameObject indicatorMinimap;

    private void Awake()
    {
        bodyPart.uniqueID = Convert.ToString(Guid.NewGuid());
        bodyPart.GetSetAnswerItemState = false;

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Easy)
            indicatorMinimap.SetActive(true);
        else
            indicatorMinimap.SetActive(false);
    }
}
