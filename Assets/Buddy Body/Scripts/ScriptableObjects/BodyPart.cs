using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Item", menuName = "Buddy Body/Body Part")]
public class BodyPart : ScriptableObject
{
    public Sprite bodyPartSprite;
    public VideoClip bodyPartVideoClip;
    [TextArea] public string description;
    public string answer;
}
