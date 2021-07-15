using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneratorDifficulty : MonoBehaviour
{
    [Header("Debugger")]
    [ReadOnly] [SerializeField] Question.Difficulty levelDifficulty;
    [ReadOnly] [SerializeField] Question.SubTopic levelSubTopic;

    // ===========================================================

    private event EventHandler changeLevelDifficulty;
    public event EventHandler onChangeLevelDifficulty
    {
        add
        {
            if (changeLevelDifficulty == null || !changeLevelDifficulty.GetInvocationList().Contains(value))
                changeLevelDifficulty += value;
        }
        remove { changeLevelDifficulty -= value; }
    }
    public Question.Difficulty GetSetDifficulty
    {
        get { return levelDifficulty; }
        set
        {
            levelDifficulty = value;
            changeLevelDifficulty?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler levelSubTopicChange;
    public event EventHandler onLevelSubTopicChange
    {
        add
        {
            if (levelSubTopicChange == null || !levelSubTopicChange.GetInvocationList().Contains(value))
                levelSubTopicChange += value;
        }
        remove { levelSubTopicChange -= value; }
    }
    public Question.SubTopic GetSetSubTopic
    {
        get { return levelSubTopic; }
        set
        {
            levelSubTopic = value;
            levelSubTopicChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
