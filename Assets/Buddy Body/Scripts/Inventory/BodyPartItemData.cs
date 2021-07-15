using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BodyPartItemData
{
    [ReadOnly] public string uniqueID;
    public BodyPart bodyPart;
    [ReadOnly] [SerializeField] bool isAnswered;

    private event EventHandler answerChange;
    public event EventHandler onAnswerChange
    {
        add
        {
            if (answerChange == null || !answerChange.GetInvocationList().Contains(value))
                answerChange += value;
        }
        remove
        {
            answerChange -= value;
        }
    }
    public bool GetSetAnswerItemState
    {
        get { return isAnswered; }
        set
        {
            isAnswered = value;
            answerChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
