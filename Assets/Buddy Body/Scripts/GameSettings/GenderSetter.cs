using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenderSetter
{
    public enum Gender
    {
        NONE,
        MALE,
        FEMALE
    }

    private event EventHandler changeGender;
    public event EventHandler onChangeGender
    {
        add
        {
            if (changeGender == null || !changeGender.GetInvocationList().Contains(value))
                changeGender += value;
        }
        remove
        {
            changeGender -= value;
        }
    }
    Gender gender;
    public Gender GetSetGender
    {
        get { return gender; }
        set
        {
            gender = value;
            changeGender?.Invoke(this, EventArgs.Empty);
        }
    }

    Animator genderAnimator;
    public Animator GetSetGenderAnimator
    {
        get { return genderAnimator; }
        set { genderAnimator = value; }
    }
}
