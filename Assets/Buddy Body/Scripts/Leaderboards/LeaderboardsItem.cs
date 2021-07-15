using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsItem : MonoBehaviour
{
    public TextMeshProUGUI rankNumber;
    public TextMeshProUGUI scoreNumber;
    public TextMeshProUGUI playerName;
    public Image barSprite;
    [ReadOnly] public string playerPassFailStats;

    public void SelectGrade()
    {
        GameManager.Instance.GetSetGradeStats = playerPassFailStats;
        GameManager.Instance.GetSetShowGradeStats = true;
    }
}
