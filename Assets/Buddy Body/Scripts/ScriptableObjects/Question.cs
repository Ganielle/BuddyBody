using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Buddy Body/Question")]
public class Question : ScriptableObject
{
    public enum Topic
    {
        NONE,
        INTERNAL,
        EXTERNAL
    }

    public enum SubTopic
    {
        NONE,
        UPPER,
        LOWER
    }

    public enum Difficulty
    {
        NONE,
        Easy,
        Medium,
        Hard
    }

    public Topic topic;
    public SubTopic subTopic;
    public Difficulty difficulty;

    [Space]
    [TextArea] public string question;
    public string answer;

    public bool stageOne;
    public bool stageTwo;
    public bool stageThree;
}
