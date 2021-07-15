using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Runtime.InteropServices;
using System;

public class ImageColorTweenAnimation : MonoBehaviour
{
    [Header("Tween Settings")]
    public LeanTweenType easeType;
    [SerializeField] private Image tweenImage;
    [SerializeField] private Color fromColor;
    [SerializeField] private Color toColor;
    [SerializeField] private float speed;
    [SerializeField] private float delay;
    [SerializeField] private bool playOnEnable;

    LTDescr imageTween;

    private void OnEnable()
    {
        if (playOnEnable)
            ImageAnimation(fromColor, toColor, easeType, speed, delay, EndTween);
    }

    public void PlayAnimation()
    {
        ImageAnimation(fromColor, toColor, easeType, speed, delay, EndTween);
    }

    public void ImageAnimation(Color fromColor, Color toColor, LeanTweenType ease, float speed, float delay, 
        [Optional] Action action)
    {
        imageTween = LeanTween.value(tweenImage.gameObject, a => tweenImage.color = a, fromColor, toColor, speed).setEase(ease).
            setDelay(delay);

        if (action != null)
            imageTween.setOnComplete(action);
    }

    private void EndTween() => LeanTween.cancel(gameObject);


}
